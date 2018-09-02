using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AI;

namespace gmtk.Player
{
    public class PlayerController : MonoBehaviour
    {
        public float MoveSpeed = 2f;
        public Conversation TestConversation;

        private Animator animator;
        private DialogueManager dialogueMan;
        private Rigidbody rbody;


        private void Start()
        {
            rbody = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();
            dialogueMan = FindObjectOfType<DialogueManager>();
        }

        private void Update()
        {
			Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
			Vector2 computedInput = input * Time.deltaTime * MoveSpeed;
			computedInput = Vector2.ClampMagnitude(computedInput, Time.deltaTime * MoveSpeed);

			animator.SetFloat("RunSpeed", computedInput.magnitude);

			Vector3 pos = transform.position;
			pos += new Vector3(computedInput.x, 0, computedInput.y);
			transform.position = pos;


			// The rigidbody keeps sliding all over the place when your dash in to things
			rbody.velocity = Vector3.zero;

			// We dont want the player Rotating if we have no motion as it defaults to 0,0,0
			if (computedInput != Vector2.zero)
			{
				Quaternion goalRotation = Quaternion.LookRotation(new Vector3(input.x, 0, input.y), Vector3.up);
				transform.rotation = Quaternion.Lerp(transform.rotation, goalRotation, Time.deltaTime * 9f);
			}


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
		}
	}
}