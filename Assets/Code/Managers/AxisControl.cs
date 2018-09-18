using System.Collections;
using System.Collections.Generic;
using SpectralDaze.Managers;
using UnityEngine;


namespace SpectralDaze.Managers.InputManager
{
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/InputManager/AxisControl")]
    public class AxisControl : ScriptableObject
    {
        public string ControlName;
        public KeyboardAxis KeyboardAxis;
        public GamepadAxis GamepadAxis;

        //[HideInInspector]
        public bool Inverted;


        public float _value;
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