using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.DialogueSystem
{
    [CreateAssetMenu(menuName = "Spectral Daze/Dialogue/Character")]
    public class CharacterInformation : ScriptableObject
    {
        public string Name;
        public Texture2D Portiate;
        public List<AudioClip> Audio;
    }
}