using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpectralDaze.EventSystem
{
    /// <summary>
    /// Scriptable Object Architecture Event Listener.
    /// </summary>
    public class EventListener : MonoBehaviour
    {
        [Tooltip("Event to register with.")]
        public Event Event;

        [Tooltip("Response to invoke when Event is raised.")]
        public UnityEvent Response;

        /// <summary>
        /// Called when [enable].
        /// </summary>
        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        /// <summary>
        /// Called when [disable].
        /// </summary>
        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        /// <summary>
        /// Called when event is raised.
        /// </summary>
        public void OnEventRaised()
        {
            Response.Invoke();
        }
    }
}

