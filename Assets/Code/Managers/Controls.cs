using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SpectralDaze.Managers.InputManager
{
    /// <summary>
    /// A collection of all the controls for the input manager
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject" />
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/InputManager/Controls")]
    public class Controls : ScriptableObject
    {
        /// <summary>
        /// A list of controls for the input manager
        /// </summary>
        public List<Control> ControlList;
        /// <summary>
        /// A list of axis controls for the input manager
        /// </summary>
        public List<AxisControl> AxisControls;
    }
}
