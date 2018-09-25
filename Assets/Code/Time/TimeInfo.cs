using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpectralDaze.Time;
using SpectralDaze.Time.Information;

namespace SpectralDaze.Time
{
    /// <summary>
    /// Time information to give other scripts for time manipulation.
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject" />
    [CreateAssetMenu(menuName = "Spectral Daze/Time/Time Info")]
    public class TimeInfo : ScriptableObject
    {
        /// <summary>
        /// The information containing speed modifier values.
        /// </summary>
        public List<InformationListItem> Data = new List<InformationListItem>(3);
    }
}