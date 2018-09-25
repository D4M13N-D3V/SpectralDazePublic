using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.DialogueSystem
{
    /// <summary>
    /// Scriptable object to ho.ld the current dialogue that is going to be useds ( SO Architecture )
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject" />
    [CreateAssetMenu(menuName = "Spectral Daze/Dialogue/DialogueQueue")]
    public class CurrentDialogue : ScriptableObject
    {
        /// <summary>
        /// The dialogue
        /// </summary>
        public TextAsset Dialogue;
    }
}