using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
	[SerializeField]
	Inputs inputs;
	[SerializeField]
	float maxDistance;
	[SerializeField]
	float longUseDelay;
	[SerializeField]
	Image crosshair;
	[SerializeField]
	LayerMask IUsableMask;

	IUsable _target;
	IUsable tmpTarget;
	float useDownTime;

	IUsable target { get => _target; set { _target = value; ChangeCrosshairState(); } }

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
		//Checking for long press
		if (useDownTime >= 0f)
		{
			if (target != null)
			{
				//Target did not change
				if (target == tmpTarget)
				{
					useDownTime += Time.deltaTime;
				}
				else
				{
					useDownTime = -1f;
					return;
				}
				//Use btn up : quick use
				if (inputs.useUp)
				{
					useDownTime = -1f;
					target.Use();
					Debug.Log("Short use");
					return;
				}
				//Holding longer than delay : LongUse
				else if (useDownTime > longUseDelay)
				{
					useDownTime = -1f;
					target.LongUse();
					Debug.Log("Long use");
					return;
				}
			}
			else
			{
				useDownTime = -1f;
				tmpTarget = null;
			}
		}
		//Checking for btn use down
		else
		{
			if (inputs.useDown && target != null)
			{
				useDownTime = 0f;
				tmpTarget = target;
			}
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