using System.Collections;
using System.Collections.Generic;
using SpectralDaze.Managers.InputManager;
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

            /// <summary>
            /// The target to follow
            /// </summary>
            public Transform Target;
            /// <summary>
            /// The smoothness of movement when following
            /// </summary>
            public float Smoothness = 0.2f;
            /// <summary>
            /// The maximum padding for x & z for the slight movement when moving mouse to edges of screen
            /// </summary>
            public Vector2 MaxPadding = new Vector2(1, 1);
            /// <summary>
            /// The minimum padding for x & z for the slight movement when moving mouse to edges of screen
            /// </summary>
            public Vector2 MinPadding = new Vector2(-1, -1);
            /// <summary>
            /// The current offset
            /// </summary>
            public Vector3 Offset = Vector3.zero;
            /// <summary>
            /// The input offset
            /// </summary>
            private Vector3 InputOffset = Vector3.zero;

            /// <summary>
            /// The camera control x axis
            /// </summary>
            public AxisControl CameraControlX;
            /// <summary>
            /// The camera control y axis
            /// </summary>
            public AxisControl CameraControlY;

            /// <summary>
            /// The current control type
            /// </summary>
            private CurrentControlType CurrentControlType;

            void Start()
            {
                CurrentControlType = Resources.Load<CurrentControlType>("Managers/InputManager/CurrentControlType");
                CameraControlX = Resources.Load<AxisControl>("Managers/InputManager/CameraControlX");
                CameraControlY = Resources.Load<AxisControl>("Managers/InputManager/CameraControlY");
            }

            void LateUpdate()
            {
                if (!Target) return;
                if (CurrentControlType.ControllerType == ControllerType.Keyboard)
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
                else
                {
                    if (CameraControlX.Value < 0)
                    {
                        InputOffset.x -= Smoothness;

                        if (InputOffset.x < MinPadding.x)
                            InputOffset.x = MinPadding.x;
                    }
                    else if (CameraControlX.Value > 0)
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

                    if (CameraControlY.Value < 0)
                    {
                        InputOffset.z -= Smoothness;

                        if (InputOffset.z < MinPadding.y)
                            InputOffset.z = MinPadding.y;
                    }
                    else if (CameraControlY.Value > 0)
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