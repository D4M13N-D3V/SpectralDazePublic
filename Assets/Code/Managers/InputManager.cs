using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.Managers.InputManager
{
    /// <summary>
    /// The types of input
    /// </summary>
    public enum InputTypes
    {
        Key,
        Mouse
    }

    /// <summary>
    /// The mouse buttons
    /// </summary>
    public enum MouseButtons
    {
        Left,
        Right,
        Middle
    }

    /// <summary>
    /// The control types
    /// </summary>
    public enum ControllerType
    {
        Keyboard,
        DS4,
        XBOX
    }

    /// <summary>
    /// The keyboard axis
    /// </summary>
    public enum KeyboardAxis
    {
        X,
        Y,
        MouseX,
        MouseY
    }

    /// <summary>
    /// The gamepade codes
    /// </summary>
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

    /// <summary>
    /// The gamepad axis
    /// </summary>
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

    /// <summary>
    /// Manages the input for the game
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class InputManager : MonoBehaviour
    {
        private bool _inputEnabled = true;

        public void SetInputEnabled(bool enabled)
        {
            _inputEnabled = enabled;
        }


        /// <summary>
        /// The types of controls being used.
        /// </summary>
        public ControllerType GamepadType = ControllerType.Keyboard;

        /// <summary>
        /// The axis for gamepad that need to be inverted.
        /// </summary>
        public List<GamepadAxis> InvertedGamepadeAxis = new List<GamepadAxis>()
        {
            GamepadAxis.LeftThumbStickY,
            GamepadAxis.RightThumbStickY,
            //GamepadAxis.DPadStickY
        };

        /// <summary>
        /// Sets up the axis enums to actual axis's in the unit input managr.
        /// </summary>
        public Dictionary<KeyboardAxis, string> KeyboardAxisReference = new Dictionary<KeyboardAxis, string>()
        {
            [KeyboardAxis.MouseX] = "Mouse X",
            [KeyboardAxis.MouseY] = "Mouse Y",
            [KeyboardAxis.X] = "Axis1",
            [KeyboardAxis.Y] = "Axis2",
        };

#if (UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX)
        public Dictionary<GamepadCode, string> XboxButtonReference = new Dictionary<GamepadCode, string>()
        {
            [GamepadCode.A] = "joystick button 16",
            [GamepadCode.B] = "joystick button 17",
            [GamepadCode.X] = "joystick button 18",
            [GamepadCode.Y] = "joystick button 19",
            [GamepadCode.LeftBumper] = "joystick button 13",
            [GamepadCode.RightBumper] = "joystick button 14",
            [GamepadCode.Back] = "joystick button 10",
            [GamepadCode.Start] = "joystick button 9",
            [GamepadCode.LeftThumbstick] = "joystick button 11",
            [GamepadCode.RightThumbstick] = "joystick button 12",
        };
        public Dictionary<GamepadAxis, string> XboxAxisReference = new Dictionary<GamepadAxis, string>()
        {
            [GamepadAxis.LeftThumbStickX] = "Axis1",
            [GamepadAxis.LeftThumbStickY] = "Axis2",
            [GamepadAxis.RightThumbStickX] = "Axis3",
            [GamepadAxis.RightThumbStickY] = "Axis4",
            [GamepadAxis.DPadStickX] = "Axis12",
            [GamepadAxis.DPadStickY] = "Axis13",
            [GamepadAxis.LeftTrigger] = "Axis5",
            [GamepadAxis.RightTrigger] = "Axis6",
        };
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
#elif (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
        public Dictionary<GamepadCode, string> XboxButtonReference = new Dictionary<GamepadCode, string>()
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
        public Dictionary<GamepadAxis, string> XboxAxisReference = new Dictionary<GamepadAxis, string>()
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
#elif UNITY_STANDALONE_LINUX
        public Dictionary<GamepadCode, string> XboxButtonReference = new Dictionary<GamepadCode, string>()
        {
            [GamepadCode.A] = "joystick button 0",
            [GamepadCode.B] = "joystick button 1",
            [GamepadCode.X] = "joystick button 2",
            [GamepadCode.Y] = "joystick button 3",
            [GamepadCode.LeftBumper] = "joystick button 4",
            [GamepadCode.RightBumper] = "joystick button 5",
            [GamepadCode.Back] = "joystick button 6",
            [GamepadCode.Start] = "joystick button 7",
            [GamepadCode.LeftThumbstick] = "joystick button 9",
            [GamepadCode.RightThumbstick] = "joystick button 10",
        };
        public Dictionary<GamepadAxis, string> XboxAxisReference = new Dictionary<GamepadAxis, string>()
        {
            [GamepadAxis.LeftThumbStickX] = "Axis1",
            [GamepadAxis.LeftThumbStickY] = "Axis2",
            [GamepadAxis.RightThumbStickX] = "Axis4",
            [GamepadAxis.RightThumbStickY] = "Axis5",
            [GamepadAxis.DPadStickX] = "Axis7,
            [GamepadAxis.DPadStickY] = "Axis8",
            [GamepadAxis.LeftTrigger] = "Axis3",
            [GamepadAxis.RightTrigger] = "Axis6",
        };
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
#endif


        /// <summary>
        /// A scriptable objec thoilding information about all configured controls.
        /// </summary>
        public Controls Controls;
        /// <summary>
        /// A scriptable object that holds information about what the current control type is.
        /// </summary>
        public CurrentControlType CurrentControlType;

        private void Start()
        {
            Controls = Resources.Load<Controls>("Managers/InputManager/Controls");
            CurrentControlType = Resources.Load<CurrentControlType>("Managers/InputManager/CurrentControlType");

            //make sure the ones that need to be inverted are set

            foreach (var axis in Controls.AxisControls)
            {
                axis.Inverted = InvertedGamepadeAxis.Contains(axis.GamepadAxis);
            }
        }

        private void Update()
        {
                string[] names = Input.GetJoystickNames();
                for (int x = 0; x < names.Length; x++)
                {
                    if (names[x].Length == 19)
                        CurrentControlType.ControllerType = ControllerType.DS4;
                    else if (names[x].Length == 33)
                        CurrentControlType.ControllerType = ControllerType.XBOX;
                    else
                        CurrentControlType.ControllerType = ControllerType.Keyboard;
                }
                foreach (var control in Controls.ControlList)
                {
                    if (CurrentControlType.ControllerType == ControllerType.Keyboard && control.IsMouseButton)
                    {
                        control.JustPressed = Input.GetMouseButton((int)control.MouseButton);
                        control.BeingPressed = Input.GetMouseButtonDown((int)control.MouseButton);
                        control.JustReleased = Input.GetMouseButtonUp((int)control.MouseButton);
                    }
                    else if (CurrentControlType.ControllerType == ControllerType.Keyboard && !control.IsMouseButton)
                    {
                        control.JustPressed = Input.GetKeyDown(control.KeyCode);
                        control.BeingPressed = Input.GetKey(control.KeyCode);
                        control.JustReleased = Input.GetKeyUp(control.KeyCode);
                    }
                    else if (CurrentControlType.ControllerType == ControllerType.DS4)
                    {
                        control.JustPressed = Input.GetKeyDown(DS4ButtonReference[control.GamepadCode]);
                        control.BeingPressed = Input.GetKey(DS4ButtonReference[control.GamepadCode]);
                        control.JustReleased = Input.GetKeyUp(DS4ButtonReference[control.GamepadCode]);
                    }
                    else if (CurrentControlType.ControllerType == ControllerType.XBOX)
                    {
                        control.JustPressed = Input.GetKeyDown(XboxButtonReference[control.GamepadCode]);
                        control.BeingPressed = Input.GetKey(XboxButtonReference[control.GamepadCode]);
                        control.JustReleased = Input.GetKeyUp(XboxButtonReference[control.GamepadCode]);
                    }
                }
                foreach (var control in Controls.AxisControls)
                {
                    if (CurrentControlType.ControllerType == ControllerType.Keyboard)
                    {
                        if (control.Inverted)
                        {
                            control.Value = Input.GetAxis(KeyboardAxisReference[control.KeyboardAxis]) * -1;
                            control.RawValue = Input.GetAxisRaw(KeyboardAxisReference[control.KeyboardAxis]) * -1;
                        }
                        else
                        {
                            control.Value = Input.GetAxis(KeyboardAxisReference[control.KeyboardAxis]);
                            control.RawValue = Input.GetAxisRaw(KeyboardAxisReference[control.KeyboardAxis]);
                        }
                    }
                    else if (CurrentControlType.ControllerType == ControllerType.DS4)
                    {
                        control.Value = Input.GetAxis(DS4AxisReference[control.GamepadAxis]);
                        control.RawValue = Input.GetAxisRaw(DS4AxisReference[control.GamepadAxis]);
                    }
                    else if (CurrentControlType.ControllerType == ControllerType.XBOX)
                    {
                        control.Value = Input.GetAxis(XboxAxisReference[control.GamepadAxis]);
                        control.RawValue = Input.GetAxisRaw(XboxAxisReference[control.GamepadAxis]);
                    }
                }
        }

    }
}