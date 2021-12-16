using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Slidable : MonoBehaviour, IUsable
{
	[SerializeField]
	UnityEvent OnUsed;
	[SerializeField]
	Vector3 movementOpen;
	[SerializeField]
	Vector3 movementClose;
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
			if (!isOpen)
			{
				t.DOLocalMove(movementOpen, tweenDuration).onComplete = () => isTweening = false;
			}
			else
			{
				t.DOLocalMove(movementClose, tweenDuration).onComplete = () => isTweening = false;
			}
			isTweening = true;
			isOpen = !isOpen;
		}
	}

	public void LongUse()
	{
		if (!isTweening && !isLocked)
		{
			if (!isOpen)
			{
				t.DOMove(movementOpen, tweenDuration * 2).onComplete = () => isTweening = false;
			}
			else
			{
				t.DOMove(movementClose, tweenDuration * 2).onComplete = () => isTweening = false;
			}
			isTweening = true;
			isOpen = !isOpen;
		}
	}
}
