using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float SelfDestructTime = 1.5f;
	void Start ()
	{
	    StartCoroutine(SelfDestructMethod());
	}

    IEnumerator SelfDestructMethod()
    {
        yield return new WaitForSeconds(SelfDestructTime);
        Destroy(gameObject);
    }

}
