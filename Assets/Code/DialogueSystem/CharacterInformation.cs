using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.DialogueSystem
{
    /// <summary>
    /// A scriptable object to hold all the characters information
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject" />
    [CreateAssetMenu(menuName = "Spectral Daze/Dialogue/Character")]
    public class CharacterInformation : ScriptableObject
    {
        /// <summary>
        /// The name of the character
        /// </summary>
        public string Name;
        /// <summary>
        /// The portiate image 
        /// </summary>
        public Texture2D Portiate;
        /// <summary>
        /// The audio clips that can play while talking.
        /// </summary>
        public List<AudioClip> Audio;
    }
}