using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Inputs : ScriptableObject
{
	public Vector2 movement;
	public Vector2 camera;
	public bool jump, sprint, sneak, sneakUp, sprintUp;

	public override string ToString()
	{
		return $"movement : {movement}, camera : {camera}, jump : {jump}, sprint : {sprint}, sneak : {sneak}";
	}
}
