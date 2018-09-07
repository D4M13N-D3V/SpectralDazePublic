using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace SpectralDaze.ScriptableObjects.Audio
{
    [CreateAssetMenu(menuName = "Spectral Daze/Audio/AudioQue")]
    public class AudioQue : ScriptableObject
    {
        public List<AudioClip> AudioClips;
    }
}