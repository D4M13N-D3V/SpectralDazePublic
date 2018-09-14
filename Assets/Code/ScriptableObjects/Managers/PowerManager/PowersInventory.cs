using System.Collections;
using System.Collections.Generic;
using SpectralDaze.Player;
using UnityEngine;

namespace SpectralDaze.ScriptableObjects.Managers.PowerManager
{
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/PowerManager/PowersInventory")]
    public class PowerInventory : ScriptableObject
    {
        public List<PlayerPower> Powers = new List<PlayerPower>();
    }

}