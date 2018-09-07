using UnityEngine;
using System.Collections;

namespace SpectralDaze.Managers
{
    class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public DialogueManager DialogueManager { get; private set; }
        public AudioManager AudioManager { get; private set; }

        void Awake()
        {
            if (Instance == null) { Instance = this; } else { Debug.Log("Warning: multiple " + this + " in scene!"); }
            DialogueManager = FindObjectOfType<DialogueManager>();
            AudioManager = FindObjectOfType<AudioManager>();
        }

        private void Start()
        {
        }
    }
}