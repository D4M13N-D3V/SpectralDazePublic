using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpectralDaze.Time;
namespace SpectralDaze.ScriptableObjects.Time
{
    [CreateAssetMenu(menuName = "Spectral Daze/Time/Time Info")]
    public class Information : ScriptableObject
    {
        /// <summary>
        /// Information for normal speed
        /// </summary>
        public SpectralDaze.Time.Information.Information Normal;
        /// <summary>
        /// Information for slow motion
        /// </summary>
        public SpectralDaze.Time.Information.Information Slowmotion;
        /// <summary>
        /// Information for fast.
        /// </summary>
        public SpectralDaze.Time.Information.Information Fast;
    }
}