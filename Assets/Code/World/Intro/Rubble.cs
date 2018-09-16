using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.World
{
    [RequireComponent(typeof(BoxCollider))]
    public class Rubble : MonoBehaviour
    {
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void Fall()
        {
            _animator.SetTrigger("RubbleFall");
            GetComponent<Collider>().enabled=false;
        }
    }
}
