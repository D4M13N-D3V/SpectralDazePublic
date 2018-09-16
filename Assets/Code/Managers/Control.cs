using System.Collections;
using System.Collections.Generic;
using SpectralDaze.Managers;
using UnityEngine;


namespace SpectralDaze.Managers.InputManager
{
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/InputManager/Control")]
    public class Control : ScriptableObject
    {
        public string ControlName;
        public bool IsMouseButton = false;
        public MouseButtons MouseButton;
        public KeyCode KeyCode;
        public bool JustPressed = false;
        public bool BeingPressed = false;
        public bool JustReleased = false;
    }
}