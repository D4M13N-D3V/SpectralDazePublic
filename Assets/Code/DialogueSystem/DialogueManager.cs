using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SpectralDaze.Managers.InputManager;
using UnityEngine;
using UnityEngine.UI;

namespace SpectralDaze.DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        public DialogueUI UI;

        public Dictionary<int, Message> Messages = new Dictionary<int, Message>();

        public List<Message> MessagesList;

        public int CurrentMessage = 0;

        public bool _dialogueOpen = false;
        public bool _optionsOpen = false;
            
        private List<Button> _optionButtons = new List<Button>();

        public Control InteractControl;

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

        private void LoadDialogue() 
        {
            Messages.Clear();
            if (CurrentDialogue == null)
            {
                Debug.LogError("No Dialogue File Currently Set, Can Not Load");
                return;
            }
            var data = JsonConvert.DeserializeObject<DialogueEditor.DialogueSave>(CurrentDialogue.Dialogue.text);
            var messages = data.Messages;
            MessagesList = messages;
            foreach (var message in messages)
            {
                Messages.Add(message.Id, message);
            }
            CurrentMessage = Messages.SingleOrDefault(x => x.Value.First).Value.Id;
        }

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

        public void StopDialogue()
        {
            UnityEngine.Time.timeScale = 1;
            UI.Parent.SetActive(false);
            StartCoroutine(Reset());
        }

        IEnumerator Reset()
        {
            yield return new WaitForSecondsRealtime(2);
            CurrentDialogue.Dialogue = null;
            _dialogueOpen = false;
        }

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