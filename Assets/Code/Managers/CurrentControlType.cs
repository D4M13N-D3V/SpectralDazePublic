using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.Managers.InputManager
{
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/InputManager/CurrentControlType")]
    public class CurrentControlType : ScriptableObject
    {
        public ControllerType ControllerType = ControllerType.Keyboard;
    }
}
