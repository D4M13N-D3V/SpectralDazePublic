using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event = SpectralDaze.EventSystem.Event;

namespace SpectralDaze.World
{
    /// <summary>
    /// Raises a event system event upon trigger enter
    /// </summary>
    public class TriggerThatTriggersEvent : MonoBehaviour
    {
        /// <summary>
        /// Event to raise.
        /// </summary>
        public Event Event;
        private void OnTriggerEnter(Collider col)
        {
            if (Event == null)
            {
                Debug.LogError("No Event Provided To Trigger");
                return;
            }

            if (col.gameObject.tag == "Player")
            {
                Event.Raise();
                Destroy(gameObject);
            }
        }
    }
}