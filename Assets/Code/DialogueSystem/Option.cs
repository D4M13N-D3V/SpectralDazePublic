using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.DialogueSystem
{
    /// <summary>
    /// A option for the next step in the dialgoue, bound to a message
    /// </summary>
    [Serializable]
    public class Option
    {
        /// <summary>
        /// The content to display for the option
        /// </summary>
        public string Content;
        /// <summary>
        /// The ID of the message to redirect to.
        /// </summary>
        public int RedirectionMessageID;
    }
}