using SpectralDaze.ScriptableObjects.Conversations;
using SpectralDaze.ScriptableObjects.Stats;
using SpectralDaze.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using SpectralDaze.ScriptableObjects.Time;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

namespace SpectralDaze.Player
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerInfo PlayerInfo;
        public Conversation TestConversation;

        public Animator Animator;

        private DialogueManager dialogueMan;
        private Rigidbody rbody;

        public Information timeInfo;

        public void EndGame()
        {

        }

        private void Start()
        {
            rbody = GetComponent<Rigidbody>();
            Animator = GetComponentInChildren<Animator>();
            dialogueMan = FindObjectOfType<DialogueManager>();
        }

        private void Update()
        {

            // this is debug / example on how to use the dialogue manager
            // replace the TestConversation with some external conversation for conversations started by npcs
            // or signs.
            if (Input.GetButtonDown("Jump"))
            {
                // this checks if we are in a dialogue if we aren't we start a new dialogue
                // else we cycle though the current conversations queue untill its over
                if (dialogueMan.IsQueueEmpty && dialogueMan.DialogueParentObj.activeSelf == false)
                    dialogueMan.StartDialogue(TestConversation);
                else
                    dialogueMan.CycleDialogue();

            }

            // Get our input
            Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            // if we are not allowed to move set input to 0,0
            if (!PlayerInfo.CanMove)
                input = Vector2.zero;


            Vector2 computedInput = input * UnityEngine.Time.deltaTime * PlayerInfo.MoveSpeed;
            computedInput = Vector2.ClampMagnitude(computedInput, UnityEngine.Time.deltaTime * PlayerInfo.MoveSpeed);

            Animator.SetFloat("RunSpeed", computedInput.magnitude);

            Vector3 pos = transform.position;
            pos += new Vector3(computedInput.x, 0, computedInput.y);
            transform.position = pos;


            // The rigidbody keeps sliding all over the place when your dash in to things
            rbody.velocity = Vector3.zero;

            // We dont want the player Rotating if we have no motion as it defaults to 0,0,0
            if (computedInput != Vector2.zero)
            {
                Quaternion goalRotation = Quaternion.LookRotation(new Vector3(input.x, 0, input.y), Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, goalRotation, UnityEngine.Time.deltaTime * 9f);
            }
        }
    }
}