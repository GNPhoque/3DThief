using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class VariableList : ScriptableObject
{
	public List<string> list;

	public VariableList()
	{
		list = new List<string>();
	}

	public override string ToString()
	{
		return list.Count == 0 ? "Inventory is empty" : string.Join(", ", list);
	}
}
