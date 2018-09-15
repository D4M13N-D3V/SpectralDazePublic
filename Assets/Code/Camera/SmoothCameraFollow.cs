using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.Camera
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    namespace SpectralDaze.Camera
    {
        public class SmoothCameraFollow : MonoBehaviour
        {

            public Transform Target;
            public float Smoothness = 0.2f;
            public Vector2 MaxPadding = new Vector2(1, 1);
            public Vector2 MinPadding = new Vector2(-1, -1);
            public Vector3 Offset = Vector3.zero;
            private Vector3 InputOffset = Vector3.zero;
            void LateUpdate()
            {
                if (Target)
                {
                    float mouseRatioX = Input.mousePosition.x / Screen.width;
                    float mouseRatioY = Input.mousePosition.y / Screen.height;

                    if (mouseRatioX < 0.2)
                    {
                        InputOffset.x -= Smoothness;

                        if (InputOffset.x < MinPadding.x)
                            InputOffset.x = MinPadding.x;
                    }
                    else if (mouseRatioX > 0.8)
                    {
                        InputOffset.x += Smoothness;

                        if (InputOffset.x > MaxPadding.x)
                            InputOffset.x = MaxPadding.x;
                    }
                    else
                    {
                        if (InputOffset.x > 0)
                            InputOffset.x -= Smoothness;
                        else if (InputOffset.x < 0)
                            InputOffset.x += Smoothness;
                    }

                    if (mouseRatioY < 0.2)
                    {
                        InputOffset.z -= Smoothness;

                        if (InputOffset.z < MinPadding.y)
                            InputOffset.z = MinPadding.y;
                    }
                    else if (mouseRatioY > 0.8)
                    {
                        InputOffset.z += Smoothness;

                        if (InputOffset.z > MaxPadding.y)
                            InputOffset.z = MaxPadding.y;
                    }
                    else
                    {
                        if (InputOffset.z > 0)
                            InputOffset.z -= Smoothness;
                        else if (InputOffset.z < 0)
                            InputOffset.z += Smoothness;
                    }

                    var tempVec = Target.position + Offset + InputOffset;
                    transform.position = Vector3.Lerp(transform.position, tempVec, Smoothness);
                }
            }
        }

    }
}