using System;
using System.Collections.Generic;
using SpectralDaze.Managers.AudioManager;
using UnityEngine;

namespace SpectralDaze.Characters
{
    [CreateAssetMenu(menuName = "Spectral Daze/Characters/Character")]
    public class Character : ScriptableObject
    {
        public string Name;
        public AudioQueue VoiceLines;
        public List<CharacterPortrait> CharacterPortraits;
    }
}
