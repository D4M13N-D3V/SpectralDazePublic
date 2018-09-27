using System.Collections;
using System.Collections.Generic;
using SpectralDaze.Player;
using UnityEngine;


namespace SpectralDaze.Managers
{
    /// <summary>
    /// The dash power slot.
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject" />
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/PowerManager/DashPower")]
    public class DashPower : ScriptableObject
    {
        /// <summary>
        /// The dash power
        /// </summary>
        public PlayerPower_Dash Power;
    }
}