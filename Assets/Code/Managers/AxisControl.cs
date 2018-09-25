using System.Collections;
using System.Collections.Generic;
using SpectralDaze.Managers;
using UnityEngine;


namespace SpectralDaze.Managers.InputManager
{
    /// <summary>
    /// Axis control for the input manager
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject" />
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/InputManager/AxisControl")]
    public class AxisControl : ScriptableObject
    {
        /// <summary>
        /// The control name
        /// </summary>
        public string ControlName;
        /// <summary>
        /// The keyboard axis
        /// </summary>
        public KeyboardAxis KeyboardAxis;
        /// <summary>
        /// The gamepad axis
        /// </summary>
        public GamepadAxis GamepadAxis;
        /// <summary>
        /// Is this axis inverted?
        /// </summary>
        public bool Inverted;

        /// <summary>
        /// The value
        /// </summary>
        public float _value;

        /// <summary>
        /// Gets or sets the value. If keyboard contol undos the inversion
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public float Value
        {
            get
            {
                if (Inverted)
                    return _value * -1;
                return _value;
            }
            set { _value = value; }
        }

        /// <summary>
        /// Gets or sets the value. If keyboard contol undos the inversion
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        private float _rawValue;    
        public float RawValue
        {

            get
            {
                if (Inverted)
                    return _rawValue * -1;
                return _rawValue;
            }
            set { _rawValue = value; }
        }
    }
}