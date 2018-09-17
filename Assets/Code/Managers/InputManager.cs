using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.Managers.InputManager
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


    public enum ControllerType
    {
        Keyboard,
        DS4,
        XBOX
    }

    public enum KeyboardAxis
    {
        X,
        Y,
        MouseX,
        MouseY
    }
    public enum GamepadCode
    {
        A,
        B,
        X,
        Y,
        LeftBumper,
        RightBumper,
        Back,
        Start,
        LeftThumbstick,
        RightThumbstick
    }
    public enum GamepadAxis
    {
        LeftTrigger,
        RightTrigger,
        LeftThumbStickX,
        LeftThumbStickY,
        DPadStickX,
        DPadStickY,
        RightThumbStickX,
        RightThumbStickY,
    }
    

    public class InputManager : MonoBehaviour
    {
        public ControllerType GamepadType = ControllerType.Keyboard;
        public List<GamepadAxis> InvertedAxis = new List<GamepadAxis>()
        {
            GamepadAxis.LeftThumbStickY,
            GamepadAxis.RightThumbStickY,
            GamepadAxis.DPadStickY
        };

        public Dictionary<KeyboardAxis, string> KeyboardAxisReference = new Dictionary<KeyboardAxis, string>()
        {
            [KeyboardAxis.MouseX] = "Mouse X",
            [KeyboardAxis.MouseY] = "Mouse Y",
            [KeyboardAxis.X] = "Axis1",
            [KeyboardAxis.Y] = "Axis2",
        };

        /*
         * XBOX SETUP FOR WINDOWS
         */
        public Dictionary<GamepadCode,string> XboxButtonReference = new Dictionary<GamepadCode, string>()
        {
            [GamepadCode.A] = "joystick button 0",
            [GamepadCode.B] = "joystick button 1",
            [GamepadCode.X] = "joystick button 2",
            [GamepadCode.Y] = "joystick button 3",
            [GamepadCode.LeftBumper] = "joystick button 4",
            [GamepadCode.RightBumper] = "joystick button 5",
            [GamepadCode.Back] = "joystick button 6",
            [GamepadCode.Start] = "joystick button 7",
            [GamepadCode.LeftThumbstick] = "joystick button 8",
            [GamepadCode.RightThumbstick] = "joystick button 9",
        };
        public Dictionary<GamepadAxis,string> XboxAxisReference = new Dictionary<GamepadAxis, string>()
        {
            [GamepadAxis.LeftThumbStickX] = "Axis1",
            [GamepadAxis.LeftThumbStickY] = "Axis2",
            [GamepadAxis.RightThumbStickX] = "Axis4",
            [GamepadAxis.RightThumbStickY] = "Axis5",
            [GamepadAxis.DPadStickX] = "Axis6",
            [GamepadAxis.DPadStickY] = "Axis7",
            [GamepadAxis.LeftTrigger] = "Axis9",
            [GamepadAxis.RightTrigger] = "Axis10",
        };
        /*
         * DS4 SETUP FOR WINDOWS
         */
        public Dictionary<GamepadCode, string> DS4ButtonReference = new Dictionary<GamepadCode, string>()
        {
            [GamepadCode.A] = "joystick button 1",
            [GamepadCode.B] = "joystick button 2",
            [GamepadCode.X] = "joystick button 0",
            [GamepadCode.Y] = "joystick button 3",
            [GamepadCode.LeftBumper] = "joystick button 4",
            [GamepadCode.RightBumper] = "joystick button 5",
            [GamepadCode.Back] = "joystick button 8",
            [GamepadCode.Start] = "joystick button 9",
            [GamepadCode.LeftThumbstick] = "joystick button 10",
            [GamepadCode.RightThumbstick] = "joystick button 11",
        };
        public Dictionary<GamepadAxis, string> DS4AxisReference = new Dictionary<GamepadAxis, string>()
        {
            [GamepadAxis.LeftThumbStickX] = "Axis1",
            [GamepadAxis.LeftThumbStickY] = "Axis2",
            [GamepadAxis.RightThumbStickX] = "Axis3",
            [GamepadAxis.RightThumbStickY] = "Axis4",
            [GamepadAxis.DPadStickX] = "Axis7",
            [GamepadAxis.DPadStickY] = "Axis8",
            [GamepadAxis.LeftTrigger] = "Axis5",
            [GamepadAxis.RightTrigger] = "Axis6",
        };


        public Controls Controls;

        private void Start()
        {
            Controls = Resources.Load<Controls>("Managers/InputManager/Controls");     

            //make sure the ones that need to be inverted are set

            foreach (var axis in Controls.AxisControls)
            {
                axis.Inverted = InvertedAxis.Contains(axis.GamepadAxis);
            }
        }

        private void Update()
        {
            string[] names = Input.GetJoystickNames();
            for (int x = 0; x < names.Length; x++)
            {
                if (names[x].Length == 19)
                    GamepadType = ControllerType.DS4;
                else if (names[x].Length == 33)
                    GamepadType = ControllerType.XBOX;
                else
                    GamepadType = ControllerType.Keyboard;
            }
            foreach (var control in Controls.ControlList)
            {
                if (GamepadType == ControllerType.Keyboard && control.IsMouseButton)
                {
                    control.JustPressed = Input.GetMouseButton((int)control.MouseButton);
                    control.BeingPressed = Input.GetMouseButtonDown((int)control.MouseButton);
                    control.JustReleased = Input.GetMouseButtonUp((int)control.MouseButton);
                }
                else if (GamepadType == ControllerType.Keyboard && !control.IsMouseButton)
                {
                    control.JustPressed = Input.GetKeyDown(control.KeyCode);
                    control.BeingPressed = Input.GetKey(control.KeyCode);
                    control.JustReleased = Input.GetKeyUp(control.KeyCode);
                }
                else if (GamepadType == ControllerType.DS4)
                {
                    control.JustPressed = Input.GetKeyDown(DS4ButtonReference[control.GamepadCode]);
                    control.BeingPressed = Input.GetKey(DS4ButtonReference[control.GamepadCode]);
                    control.JustReleased = Input.GetKeyUp(DS4ButtonReference[control.GamepadCode]);
                }
                else if (GamepadType == ControllerType.XBOX)
                {
                    control.JustPressed = Input.GetKeyDown(XboxButtonReference[control.GamepadCode]);
                    control.BeingPressed = Input.GetKey(XboxButtonReference[control.GamepadCode]);
                    control.JustReleased = Input.GetKeyUp(XboxButtonReference[control.GamepadCode]);
                }
            }
            foreach (var control in Controls.AxisControls)
            {
                if (GamepadType == ControllerType.Keyboard)
                {
                    control.Value = Input.GetAxis(KeyboardAxisReference[control.KeyboardAxis]);
                    control.Value = Input.GetAxisRaw(KeyboardAxisReference[control.KeyboardAxis]);
                }
                else if (GamepadType == ControllerType.DS4)
                {
                    control.Value = Input.GetAxis(DS4AxisReference[control.GamepadAxis]);
                    control.Value = Input.GetAxisRaw(DS4AxisReference[control.GamepadAxis]);
                }
                else if (GamepadType == ControllerType.XBOX)
                {
                    control.Value = Input.GetAxis(XboxAxisReference[control.GamepadAxis]);
                    control.Value = Input.GetAxisRaw(XboxAxisReference[control.GamepadAxis]);
                }
            }
        }

    }
}