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

			Quaternion rotation = Quaternion.identity;
			if(Math.Abs(input.magnitude) > 0.01f)
				rotation = Quaternion.LookRotation(new Vector3(input.x, 0, input.y), Vector3.up);
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 9f);
		}
	}
}