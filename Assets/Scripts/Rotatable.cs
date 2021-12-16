using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Rotatable : MonoBehaviour, IUsable
{
	[SerializeField]
	UnityEvent OnUsed;
	[SerializeField]
	Vector3 rotation;
	[SerializeField]
	float tweenDuration;
	[SerializeField]
	bool defaultActive;

	Transform t;
	bool isOpen;
	bool isLocked;
	bool isTweening;

	private void Awake()
	{
		t = GetComponent<Transform>();
	}

	public void Use()
	{
		if (!isTweening && !isLocked)
		{
			if (isOpen)
			{
				t.DORotate(rotation, tweenDuration, RotateMode.LocalAxisAdd).onComplete = () => isTweening = false;
			}
			else
			{
				t.DORotate(-rotation, tweenDuration, RotateMode.LocalAxisAdd).onComplete = () => isTweening = false;
			}
			isTweening = true;
			isOpen = !isOpen;
		}
	}

	public void LongUse()
	{
		Use();
	}
}
