using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Shows the amount of gameobjects in the scene.
/// </summary>
public class ObjectCount : MonoBehaviour
{
	private void OnGUI()
	{
		GUI.Label(new Rect(0, 0, 100, 100), GameObject.FindObjectsOfType<GameObject>().Length + "");
	}
}