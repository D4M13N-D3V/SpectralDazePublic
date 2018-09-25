using System.Collections;
using System.Collections.Generic;
using SpectralDaze.Managers;
using UnityEngine;


namespace SpectralDaze.Managers.InputManager
{
    /// <summary>
    /// A control for the input manager.
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject" />
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/InputManager/Control")]
    public class Control : ScriptableObject
    {
        /// <summary>
        /// The name of the control.
        /// </summary>
        public string ControlName;

        /// <summary>
        /// Is this control a mouse button for keyboard controls??
        /// </summary>
        public bool IsMouseButton = false;
        public MouseButtons MouseButton;

        /// <summary>
        /// The key code for the input for this control
        /// </summary>
        public KeyCode KeyCode;
        /// <summary>
        /// The gamepad code for the input for this control
        /// </summary>
        public GamepadCode GamepadCode;

        /// <summary>
        /// Is the control just pressed
        /// </summary>
        public bool JustPressed = false;
        /// <summary>
        /// Is the control being pressed.
        /// </summary>
        public bool BeingPressed = false;
        /// <summary>
        /// Is the contrl just released.
        /// </summary>
        public bool JustReleased = false;
    }
}