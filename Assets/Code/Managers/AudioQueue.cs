using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SpectralDaze.Managers.AudioManager
{
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/Audio/AudioQueue")]
    public class AudioQueue : ScriptableObject
    {
        public Queue<AudioClipInfo> Queue = new Queue<AudioClipInfo>();
    }
}