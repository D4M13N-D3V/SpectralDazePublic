using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SpectralDaze.Managers.AudioManager
{
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/Audio/AudioClipInfo")]
    public class AudioClipInfo : ScriptableObject
    {
        public bool Loop;
        public float Duration;
        public float Volume;
        public AudioClip Clip;
    }
}
