using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.Managers.InputManager
{
    /// <summary>
    /// The type of controls currently being used.
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject" />
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/InputManager/CurrentControlType")]
    public class CurrentControlType : ScriptableObject
    {
        /// <summary>
        /// The controller type currently being used in enum form.
        /// </summary>
        public ControllerType ControllerType = ControllerType.Keyboard;
    }
}
