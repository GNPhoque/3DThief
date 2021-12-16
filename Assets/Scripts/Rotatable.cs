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
	bool collided;

	private void Awake()
	{
		t = GetComponent<Transform>();
	}

	private void Update()
	{
		//Debug.Log(isTweening);
	}

	public void Use()
	{
		if (!isTweening && !isLocked)
		{
			if (isOpen)
			{
				t.DORotate(rotation, tweenDuration, RotateMode.LocalAxisAdd).onComplete = () =>
				{
					Debug.Log($"Rotation ended, collided = {collided}");
					isTweening = false;
					if (collided)
					{
						collided = false;
						Use();
					}
				};
			}
			else
			{
				t.DORotate(-rotation, tweenDuration, RotateMode.LocalAxisAdd).onComplete = () =>
				{
					Debug.Log($"Rotation ended, collided = {collided}");
					isTweening = false;
					if (collided)
					{
						collided = false;
						Use();
					}
				};
			}
			isTweening = true;
			isOpen = !isOpen;
		}
	}

	public void LongUse()
	{
		if (!isTweening && !isLocked)
		{
			if (isOpen)
			{
				t.DORotate(rotation, tweenDuration * 3f, RotateMode.LocalAxisAdd).onComplete = () =>
				{
					Debug.Log($"Rotation ended, collided = {collided}");
					isTweening = false;
					if (collided)
					{
						collided = false;
						Use();
					}
				};
			}
			else
			{
				t.DORotate(-rotation, tweenDuration * 3f, RotateMode.LocalAxisAdd).onComplete = () =>
				{
					Debug.Log($"Rotation ended, collided = {collided}");
					isTweening = false;
					if (collided)
					{
						collided = false;
						Use();
					}
				};
			}
			isTweening = true;
			isOpen = !isOpen;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (isTweening && isOpen && collision.transform.CompareTag("Player"))
		{
			collided = true;
		}
	}
}
