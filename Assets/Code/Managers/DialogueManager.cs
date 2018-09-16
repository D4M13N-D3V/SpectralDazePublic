using SpectralDaze.DataTypes.Conversations;
using System;
using System.Collections.Generic;
using System.Linq;
using SpectralDaze.Characters;
using SpectralDaze.Player;
using UnityEngine;
using UnityEngine.UI;

namespace SpectralDaze.Managers
{
    public class DialogueManager : MonoBehaviour
    {
        public Image CharacterImage { get; set; }
        public Text CharacterName { get; set; }
        public Text DialogueText { get; set; }

        public GameObject DialogueParentObj;
        public Animator TextAnimator;
        public AudioClip NextDialogueSfxClip;
        public AudioSource myAudioSource;

        public bool IsDialogueActive = false;

        DateTime OnEnterTime;
        AudioClip tmpAudioClip;
        PlayerInfo playerInfo;

        private Queue<ConversationLine> ConversationQueue;


        public bool IsQueueEmpty
        {
            get
            {
                if (!ConversationQueue.Any()) return true;
                else
                    return false;
            }
        }

        private void Start()
        {
            CharacterImage = GameObject.FindGameObjectWithTag("CharacterPortrait").GetComponent<Image>();
            CharacterName = GameObject.FindGameObjectWithTag("CharacterName").GetComponent<Text>();
            DialogueText = GameObject.FindGameObjectWithTag("DialogueText").GetComponent<Text>();
            TextAnimator = GameObject.FindGameObjectWithTag("DialogueText").GetComponent<Animator>();
            myAudioSource = GetComponent<AudioSource>();

            playerInfo = Resources.Load<PlayerInfo>("Player/DefaultPlayerInfo");

            DialogueParentObj.SetActive(false);
            ConversationQueue = new Queue<ConversationLine>();
        }

        public void StartDialogue(Conversation dialogue)
        {
            ConversationQueue = new Queue<ConversationLine>(dialogue.ConversationLines);
            OnEnterTime = DateTime.Now;
            CycleDialogue();
        }


        public void DisplayText()
        {
            ShowDialogueBox();
            TextAnimator.Play("Anim_NextDialogue");

            var Line = ConversationQueue.Dequeue();

            CharacterImage.sprite = GetCharacterSprite(Line);
            CharacterName.text = Line.LineCharacter.Name;
            DialogueText.text = Line.Text;
            PlayVoice(Line.LineCharacter);
        }

        private Sprite GetCharacterSprite(ConversationLine line)
        {
            CharacterPortrait charPort = line.LineCharacter.CharacterPortraits.First(x => x.Emotion == line.LineEmotion);
            if (charPort)
                return charPort.Sprite;
            return null;
        }

        private void PlayVoice(Character character)
        {
            var rnd = new System.Random();
            var lastVoice = tmpAudioClip;

            var i = 0;
            while (lastVoice == tmpAudioClip && i < 10)
            {
                //tmpAudioClip = character.VoiceLines.AudioClips[rnd.Next(0, character.VoiceLines.AudioClips.Count)];
                i++;
            }

            myAudioSource.Stop();
            myAudioSource.clip = tmpAudioClip;
            myAudioSource.Play();
        }

        public void ShowDialogueBox()
        {
            DialogueParentObj.SetActive(true);
            playerInfo.CanMove = false;
        }

        public void HideDialogueBox()
        {
            DialogueParentObj.SetActive(false);
            playerInfo.CanMove = true;
        }

        public void CycleDialogue()
        {
            if (IsQueueEmpty)
                HideDialogueBox();
            else
                DisplayText();
        }
    }
}