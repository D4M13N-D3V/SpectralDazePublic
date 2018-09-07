using SpectralDaze.ScriptableObjects.Characters;
using System;
using UnityEngine;

namespace SpectralDaze.DataTypes.Conversations
{
    [Serializable]
    public class ConversationLine
    {
        public Character LineCharacter;
        [TextArea(3, 10)]
        public string Text;
        public Emotion LineEmotion;
    }
}
