using System.Collections;
using System.Collections.Generic;
using SpectralDaze.Player;
using UnityEngine;


namespace SpectralDaze.Managers
{
    /// <summary>
    /// Power slot 1
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject" />
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/PowerManager/Power1")]
    public class Power1 : ScriptableObject
    {
        /// <summary>
        /// The power
        /// </summary>
        public PlayerPower Power;
    }
}