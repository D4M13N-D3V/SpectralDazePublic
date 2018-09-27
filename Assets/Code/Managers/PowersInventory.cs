using System.Collections;
using System.Collections.Generic;
using SpectralDaze.Player;
using UnityEngine;

namespace SpectralDaze.Managers
{
    /// <summary>
    /// All the powers available to the player
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject" />
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/PowerManager/PowersInventory")]
    public class PowerInventory : ScriptableObject
    {
        /// <summary>
        /// The powers the player has access to.
        /// </summary>
        public List<PlayerPower> Powers = new List<PlayerPower>();
    }

}