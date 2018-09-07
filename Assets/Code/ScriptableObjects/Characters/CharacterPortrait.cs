using System;
using System.Collections.Generic;
using UnityEngine;
using SpectralDaze.ScriptableObjects.Audio;

namespace SpectralDaze.ScriptableObjects.Characters
{
    [CreateAssetMenu(menuName = "Spectral Daze/Characters/Character Portrait")]
    public class CharacterPortrait : ScriptableObject
    {
        public Emotion Emotion;
        public Sprite Sprite;
    }
}
