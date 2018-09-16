using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.Characters
{
    [CreateAssetMenu(menuName = "Spectral Daze/Characters/Character Portrait")]
    public class CharacterPortrait : ScriptableObject
    {
        public Emotion Emotion;
        public Sprite Sprite;
    }
}
