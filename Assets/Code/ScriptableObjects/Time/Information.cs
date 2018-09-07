using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpectralDaze.Time;
namespace SpectralDaze.ScriptableObjects.Time
{
    public class Information : ScriptableObject
    {
        public SpectralDaze.Time.Information.Information Normal;
        public SpectralDaze.Time.Information.Information Slowmotion;
        public SpectralDaze.Time.Information.Information Fast;
    }
}