using System;
using System.Collections.Generic;
using SpectralDaze.ScriptableObjects.Managers.Audio;
using UnityEngine;

namespace SpectralDaze.ScriptableObjects.Characters
{
    [CreateAssetMenu(menuName = "Spectral Daze/Characters/Character")]
    public class Character : ScriptableObject
    {
        public string Name;
        public AudioQueue VoiceLines;
        public List<CharacterPortrait> CharacterPortraits;
    }
}
