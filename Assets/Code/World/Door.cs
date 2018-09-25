using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.World
{
    /// <summary>
    /// A door that can be opened.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class Door : MonoBehaviour
    {
        /// <summary>
        /// Is the door opened by default?
        /// </summary>
        public bool OpenByDefault = true;

        /// <summary>
        /// The animator
        /// </summary>
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            if(OpenByDefault)
                Open();
        }

        /// <summary>
        /// Closes the door.
        /// </summary>
        public void Close()
        {
            _animator.SetBool("DoorOpen", false);
        }

        /// <summary>
        /// Opens the door.
        /// </summary>
        public void Open()
        {
            _animator.SetBool("DoorOpen", true);
        }
    }
}