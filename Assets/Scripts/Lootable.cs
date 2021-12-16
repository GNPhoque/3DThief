using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootable : MonoBehaviour, IUsable
{
	[SerializeField]
	VariableList inventory;

	public void Use()
	{
		inventory.list.Add(name);
		Destroy(gameObject);
	}
	public void LongUse()
	{
		Use();
	}
}
