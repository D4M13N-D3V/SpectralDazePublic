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

        private Animator animator;

        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
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

            // We dont want the player Rotating if we hav no motion as it defaults to 0,0,0
            if (computedInput != Vector2.zero)
            {
                Quaternion goalRotation = Quaternion.LookRotation(new Vector3(input.x, 0, input.y), Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, goalRotation, Time.deltaTime * 9f);
            }
        }
    }
}