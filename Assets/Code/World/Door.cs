using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.World
{
    public class Door : MonoBehaviour
    {
        public bool OpenByDefault = true;

        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void Close()
        {
            _animator.SetBool("DoorOpen", false);
        }

        public void Open()
        {
            _animator.SetBool("DoorOpen", true);
        }
    }
}