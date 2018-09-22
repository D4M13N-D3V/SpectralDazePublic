using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace SpectralDaze.DialogueSystem
{
    [Serializable]  
    public class Message
    {
        public int Id;
        public string Content;
        public List<Option> Options;

        public string CharacterPath
        {
            get { return _characterPath; }
            set
            {
                Character = Resources.Load<CharacterInformation>(value.Replace(".asset", "").Replace("Assets/Resources/", ""));
                _characterPath = value;
            }
        }
        private string _characterPath;

        [JsonIgnore] public CharacterInformation Character;
        public bool First;
        public bool Last;

    }
}