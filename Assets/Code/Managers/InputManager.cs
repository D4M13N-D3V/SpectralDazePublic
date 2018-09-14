using System.Collections;
using System.Collections.Generic;
using SpectralDaze.ScriptableObjects.Managers.InputManager;
using UnityEngine;

namespace SpectralDaze.Managers
{
    public enum InputTypes
    {
        Key,
        Mouse
    }

    public enum MouseButtons
    {
        Left,
        Right,
        Middle
    }

    public class InputManager : MonoBehaviour
    {
        public Controls Controls;

        private void Start()
        {
            Controls = Resources.Load<Controls>("Managers/InputManager/Controls");     
        }

        private void Update()
        {
            foreach (var control in Controls.ControlList)
            {
                if (control.IsMouseButton)
                {
                    control.JustPressed = Input.GetMouseButton((int)control.MouseButton);
                    control.BeingPressed = Input.GetMouseButtonDown((int)control.MouseButton);
                    control.JustReleased = Input.GetMouseButtonUp((int)control.MouseButton);
                }
                else
                {
                    control.JustPressed = Input.GetKeyDown(control.KeyCode);
                    control.BeingPressed = Input.GetKey(control.KeyCode);
                    control.JustReleased = Input.GetKeyUp(control.KeyCode);
                }
            }
        }
    }
}