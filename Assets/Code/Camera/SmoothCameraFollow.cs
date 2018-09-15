using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.Camera
{
    public class SmoothCameraFollow : MonoBehaviour
    {

        public Transform Target;
        public float Smoothness = 0.2f;
        public Vector3 MaxPadding = new Vector3(1,1,1);
        public Vector3 MinPadding = new Vector3(-1,-1,-1);
        public Vector3 Offset = Vector3.zero;
        private Vector3 InputOffset = Vector3.zero;
        void LateUpdate()
        {
            if (Target)
            {
                Debug.Log(Input.GetAxis("Mouse X"));
                Debug.Log(Input.GetAxis("Mouse Y"));
                InputOffset += new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"));

                if (InputOffset.x > MaxPadding.x)
                    InputOffset.x = MaxPadding.x;

                if (InputOffset.z > MaxPadding.z)
                    InputOffset.z = MaxPadding.z;

                if (InputOffset.x < MinPadding.x)
                    InputOffset.x = MinPadding.x;

                if (InputOffset.z < MinPadding.z)
                    InputOffset.z = MinPadding.z;

                var tempVec = Target.position + Offset + InputOffset;
                transform.position = Vector3.Lerp(transform.position, tempVec, Smoothness);
            }
        }
    }

}