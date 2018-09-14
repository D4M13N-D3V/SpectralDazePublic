using System.Collections;
using System.Collections.Generic;
using SpectralDaze.Player;
using UnityEngine;


namespace SpectralDaze.ScriptableObjects.Managers.PowerManager
{
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/PowerManager/DashPower")]
    public class DashPower : ScriptableObject
    {
        public PlayerPower_Dash Power;
    }
}