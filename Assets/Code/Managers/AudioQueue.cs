using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SpectralDaze.Managers.AudioManager
{
    /// <summary>
    /// The queue of audio clip information.
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject" />
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/Audio/AudioQueue")]
    public class AudioQueue : ScriptableObject
    {
        /// <summary>
        /// Queue of clips to play.
        /// </summary>
        public Queue<AudioClipInfo> Queue = new Queue<AudioClipInfo>();
    }
}