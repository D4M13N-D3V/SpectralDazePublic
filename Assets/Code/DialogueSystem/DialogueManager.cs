using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SpectralDaze.Managers.InputManager;
using UnityEngine;
using UnityEngine.UI;

namespace SpectralDaze.DialogueSystem
{
    /// <summary>
    /// A monbo behaviour that is modular and manages dialogue ingame
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class DialogueManager : MonoBehaviour
    {
        /// <summary>
        /// The DialogueUI monobehaviour that holds all the information for the UI needed.
        /// </summary>
        public DialogueUI UI;

        /// <summary>
        /// The messages that have been loaded from the current dialogue.
        /// </summary>
        public Dictionary<int, Message> Messages = new Dictionary<int, Message>();

        /// <summary>
        /// The current message ID
        /// </summary>
        public int CurrentMessage = 0;

        /// <summary>
        /// Is the dialogue open
        /// </summary>
        public bool _dialogueOpen = false;
        /// <summary>
        /// Are the options open.
        /// </summary>
        public bool _optionsOpen = false;

        /// <summary>
        /// The option buttons
        /// </summary>
        private List<Button> _optionButtons = new List<Button>();

        /// <summary>
        /// The interact control
        /// </summary>
        public Control InteractControl;

        /// <summary>
        /// The current dialogue
        /// </summary>
        public CurrentDialogue CurrentDialogue;
        

        private void Start()
        {
            InteractControl = Resources.Load<Control>("Managers/InputManager/Interact");

            var prefab = Resources.Load<GameObject>("DialogueSystem/Dialogue");
            var obj = Instantiate(prefab);
            UI = obj.GetComponent<DialogueUI>();
            UI.gameObject.transform.parent = GameObject.FindGameObjectWithTag("UIRoot").transform;

            CurrentDialogue = Resources.Load<CurrentDialogue>("DialogueSystem/CurrentDialogue");
            StopDialogue();
        }

        private void Update()
        {
            if (CurrentDialogue.Dialogue != null && !_dialogueOpen)
            {
                StartDialogue(CurrentDialogue.Dialogue);
            }

            if (InteractControl.JustPressed)
            {
                if (_dialogueOpen)
                    CycleDialogue();
            }
        }

        /// <summary>
        /// Loads the dialogue that current dialogue is set to.
        /// </summary>
        private void LoadDialogue() 
        {
            Messages.Clear();
            if (CurrentDialogue == null)
            {
                Debug.LogError("No Dialogue File Currently Set, Can Not Load");
                return;
            }
            var data = JsonConvert.DeserializeObject<DialogueSave>(CurrentDialogue.Dialogue.text);
            var messages = data.Messages;
            foreach (var message in messages)
            {
                Messages.Add(message.Id, message);
            }
            CurrentMessage = Messages.SingleOrDefault(x => x.Value.First).Value.Id;
        }

        /// <summary>
        /// Starts the dialogue.
        /// </summary>
        /// <param name="newDialogue">The new dialogue.</param>
        public void StartDialogue(TextAsset newDialogue)
        {
            LoadDialogue();
            UnityEngine.Time.timeScale = 0;
            UI.Parent.SetActive(true);
            LoadDialogue();
            UI.MessageText.text = Messages[CurrentMessage].Content;
            UI.CharacterImage.texture = Messages[CurrentMessage].Character.Portiate;    
            UI.CharacterName.text = Messages[CurrentMessage].Character.Name;
            _dialogueOpen = true;
        }

        /// <summary>
        /// Stops the dialogue.
        /// </summary>
        public void StopDialogue()
        {
            UnityEngine.Time.timeScale = 1;
            UI.Parent.SetActive(false);
            StartCoroutine(Reset());
        }

        /// <summary>
        /// Resets the dialogue manager so you are able to open a dialogue again.
        /// </summary>
        /// <returns></returns>
        IEnumerator Reset()
        {
            yield return new WaitForSecondsRealtime(2);
            CurrentDialogue.Dialogue = null;
            _dialogueOpen = false;
        }

        /// <summary>
        /// Cycles the dialogue.
        /// </summary>
        public void CycleDialogue()
        {
            if (Messages.Values.Where(x => x.Last).ToList().Contains(Messages[CurrentMessage]))
            {
                StopDialogue();
                return;
            }
            if (!_optionsOpen)
            {
                Options();
            }
        }

        /// <summary>
        /// Loads all the option buttons.
        /// </summary>
        public void Options()
        {
            if (!_optionsOpen)
                foreach (var option in Messages[CurrentMessage].Options)
                {
                    var button = Instantiate(Resources.Load("DialogueSystem/Option") as GameObject);
                    button.transform.GetChild(0).GetComponent<Text>().text = option.Content;
                    button.transform.parent = UI.OptionsViewButtonContainer.transform;
                    var buttonComp = button.GetComponent<Button>();
                    buttonComp.onClick.AddListener(delegate
                    {
                        CurrentMessage = option.RedirectionMessageID;
                        OptionsButton();
                    });
                    _optionButtons.Add(button.GetComponent<Button>());
                }
            _optionsOpen = true;
            UI.MessageView.SetActive(false);
            UI.OptionsView.SetActive(true);
        }

        /// <summary>
        /// Called when a option button is pressed.
        /// </summary>
        public void OptionsButton()
        {
            foreach (var button in _optionButtons)
            {
                Destroy(button.gameObject);
            }
            _optionButtons.Clear();

            _optionsOpen = false;
            UI.MessageView.SetActive(true);
            UI.OptionsView.SetActive(false);
            UI.MessageText.text = Messages[CurrentMessage].Content;
            UI.CharacterImage.texture = Messages[CurrentMessage].Character.Portiate;
            UI.CharacterName.text = Messages[CurrentMessage].Character.Name;
        }
    }
}