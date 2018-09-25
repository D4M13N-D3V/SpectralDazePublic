using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.EventSystem
{
    [CreateAssetMenu(menuName = "Spectral Daze/Events/Event")]
    public class Event : ScriptableObject
    {
        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        private readonly List<EventListener> eventListeners =
            new List<EventListener>();

        /// <summary>
        /// Raises event.
        /// </summary>
        public void Raise()
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised();
        }

        /// <summary>
        /// Registers the listener.
        /// </summary>
        /// <param name="listener">The listener.</param>
        public void RegisterListener(EventListener listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        /// <summary>
        /// Unregisters the listener.
        /// </summary>
        /// <param name="listener">The listener.</param>
        public void UnregisterListener(EventListener listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }

}