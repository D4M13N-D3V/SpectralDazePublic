using System.Collections;
using System.Collections.Generic;
using SpectralDaze.Time;
using UnityEngine;

namespace SpectralDaze.Time
{
    public class TimeBubbleController : MonoBehaviour
    {
        public Manipulations Type = Manipulations.Slow;
        public List<string> TimeManipulateableTags;
        public Vector3 BubbleScale = new Vector3(7, 7, 7);

        private void Start()
        {
            transform.localScale = Vector3.zero;
            LeanTween.scale(gameObject, BubbleScale, 0.5f);
        }

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