using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SpectralDaze.Managers.AudioManager
{
    /// <summary>
    /// Audio clip information that can be qued.
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject" />
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/Audio/AudioClipInfo")]
    public class AudioClipInfo : ScriptableObject
    {
        /// <summary>
        /// Should this clip loop?
        /// </summary>
        public bool Loop;
        /// <summary>
        /// How long should this audio clip player?
        /// </summary>
        public float Duration;
        /// <summary>
        /// The volume the clip should be played at.
        /// </summary>
        public float Volume;
        /// <summary>
        /// The clip to play.
        /// </summary>
        public AudioClip Clip;
    }
}
