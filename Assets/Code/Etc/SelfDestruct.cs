using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When added causes the gameobject to self destruct.
/// </summary>
public class SelfDestruct : MonoBehaviour
{
    /// <summary>
    /// The time until the game object self destructs.
    /// </summary>
    public float SelfDestructTime = 1.5f;
	void Start ()
	{
	    StartCoroutine(SelfDestructMethod());
	}

    /// <summary>
    /// Destroys gameobject after SelfDestructTime has passed.
    /// </summary>
    /// <returns></returns>
    IEnumerator SelfDestructMethod()
    {
        yield return new WaitForSeconds(SelfDestructTime);
        Destroy(gameObject);
    }

}
