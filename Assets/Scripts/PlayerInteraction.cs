using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
	[SerializeField]
	float maxDistance;
	[SerializeField]
	Image crosshair;
	[SerializeField]
	LayerMask IUsableMask;

	IUsable target;

	private void Update()
	{
		FindTarget();
		UseTarget();
		ChangeCrosshairState();
	}

	void FindTarget()
	{
		RaycastHit hit;
		if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance, IUsableMask))
		{
			target = hit.collider.gameObject.GetComponent<IUsable>() ?? null;
		}
		else target = null;
	}

	void UseTarget()
	{
		if (Input.GetButtonDown("Use") && target != null)
		{
			target.Use();
		}
	}

	void ChangeCrosshairState()
	{
		if (target != null)
		{
			crosshair.material.color = Color.red;
		}
		else
		{
			crosshair.material.color = Color.white;
		}
	}
}