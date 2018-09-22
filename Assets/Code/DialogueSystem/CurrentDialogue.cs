using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.DialogueSystem
{
    [CreateAssetMenu(menuName = "Spectral Daze/Dialogue/DialogueQueue")]
    public class CurrentDialogue : ScriptableObject
    {
        public TextAsset Dialogue;
    }
}