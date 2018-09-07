using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour {

    public Transform Target;

    public float Smoothness = .2f;
    public Vector3 Offset = Vector3.zero;

    void LateUpdate()
    {
        if (Target)
        {
            var tempVec = Target.position + Offset;
            transform.position = Vector3.Lerp(transform.position, tempVec, Smoothness);
        }
    }
}
