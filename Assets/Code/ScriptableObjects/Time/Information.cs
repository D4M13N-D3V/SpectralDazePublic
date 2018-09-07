using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpectralDaze.Time;
using SpectralDaze.Time.Information;

namespace SpectralDaze.ScriptableObjects.Time
{
    [CreateAssetMenu(menuName = "Spectral Daze/Time/Time Info")]
    public class Information : ScriptableObject
    {
        public List<InformationListItem> Data = new List<InformationListItem>(3);
    }
}