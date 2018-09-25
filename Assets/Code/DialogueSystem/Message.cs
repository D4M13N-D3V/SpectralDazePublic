using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace SpectralDaze.DialogueSystem
{
    /// <summary>
    /// A message for the dialogue system
    /// </summary>
    [Serializable]  
    public class Message
    {
        /// <summary>
        /// The identifier
        /// </summary>
        public int Id;
        /// <summary>
        /// The content to display in the message
        /// </summary>
        public string Content;
        /// <summary>
        /// A list of options bound to hte message
        /// </summary>
        public List<Option> Options;

        /// <summary>
        /// Gets or sets the character path.
        /// </summary>
        /// <value>
        /// The character path.
        /// </value>
        public string CharacterPath
        {
            get { return _characterPath; }
            set
            {
                Character = Resources.Load<CharacterInformation>(value.Replace(".asset", "").Replace("Assets/Resources/", ""));
                _characterPath = value;
            }
        }
        /// <summary>
        /// The character path
        /// </summary>
        private string _characterPath;

        /// <summary>
        /// The character information scriptable object bound to the message.
        /// </summary>
        [JsonIgnore] public CharacterInformation Character;
        /// <summary>
        /// Is this the first node
        /// </summary>
        public bool First;
        /// <summary>
        /// The one of the ending nodes
        /// </summary>
        public bool Last;

    }
}