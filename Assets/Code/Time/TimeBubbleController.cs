using System.Collections;
using System.Collections.Generic;
using SpectralDaze.Time;
using UnityEngine;

namespace SpectralDaze.Time
{
    /// <summary>
    /// Controls the time bubble.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class TimeBubbleController : MonoBehaviour
    {
        /// <summary>
        /// The type of time manipulation to apply
        /// </summary>
        public Manipulations Type = Manipulations.Slow;

        /// <summary>
        /// A list of strings representing tags that can be time manipulated
        /// </summary>
        public List<string> TimeManipulateableTags;

        /// <summary>
        /// The scale to scale up to on creation.
        /// </summary>
        public Vector3 BubbleScale = new Vector3(7, 7, 7);

        private void Start()
        {
            transform.localScale = Vector3.zero;
            LeanTween.scale(gameObject, BubbleScale, 0.5f);
        }

        /// <summary>
        /// Destroys self after shrinking to nothing.
        /// </summary>
        public void SelfDestruct()
        {
            LeanTween.scale(gameObject, Vector3.one, 0.5f).setOnComplete(() =>
                {
                    LeanTween.move(gameObject, -transform.up * 10, 0.1f).setOnComplete(() => { Destroy(gameObject); });
                });
        }

        void OnTriggerEnter(Collider other)
        {
            if (TimeManipulateableTags.Contains(other.gameObject.tag))
            {
                other.gameObject.SendMessage("StartTimeManipulation", (int)Type);
            }
        }
        void OnTriggerExit(Collider other)
        {
            if (TimeManipulateableTags.Contains(other.gameObject.tag))
            {
                other.gameObject.SendMessage("StopTimeManipulation");
            }
        }
    }

}