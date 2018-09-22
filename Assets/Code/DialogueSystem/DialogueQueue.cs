using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.DialogueSystem
{
    [CreateAssetMenu(menuName = "Spectral Daze/Dialogue/DialogueQueue")]
    public class DialogueQueue : ScriptableObject
    {
        public Queue<TextAsset> Dialogues = new Queue<TextAsset>(); 
    }
}