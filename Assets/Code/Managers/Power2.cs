using System.Collections;
using System.Collections.Generic;
using SpectralDaze.Player;
using UnityEngine;


namespace SpectralDaze.Managers.PowerManager
{
    /// <summary>
    /// The second power slot.
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject" />
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/PowerManager/Power2")]
    public class Power2 : ScriptableObject
    {
        /// <summary>
        /// The power
        /// </summary>
        public PlayerPower Power;
    }
}