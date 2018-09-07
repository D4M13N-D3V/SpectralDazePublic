using System;
using System.Collections.Generic;
using UnityEngine;
using SpectralDaze.ScriptableObjects.Audio;

namespace SpectralDaze.ScriptableObjects.Characters
{
    [CreateAssetMenu(menuName = "Spectral Daze/Characters/Character")]
    public class Character : ScriptableObject
    {
        public string Name;
        public AudioQue VoiceLines;
        public List<CharacterPortrait> CharacterPortraits;
    }
}
