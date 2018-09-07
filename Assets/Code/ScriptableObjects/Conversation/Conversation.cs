using UnityEngine;
using System.Collections.Generic;
using SpectralDaze.DataTypes.Conversations;
using System;

namespace SpectralDaze.ScriptableObjects.Conversations {
    [CreateAssetMenu(menuName = "Spectral Daze/Conversations/Conversation")]
    public class Conversation : ScriptableObject
    {
        
        public List<ConversationLine> ConversationLines;
    }
}