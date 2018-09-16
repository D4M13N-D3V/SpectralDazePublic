using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.Events
{
    [CreateAssetMenu(menuName = "Spectral Daze/Events/Event")]
    public class Event : ScriptableObject
    {
        /// <summary>
        /// The list of listeners that this event will notify if it is raised.
        /// </summary>
        private readonly List<EventListener> eventListeners =
            new List<EventListener>();

        public void Raise()
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised();
        }

        public void RegisterListener(EventListener listener)
        {
            if (!eventListeners.Contains(listener))
                eventListeners.Add(listener);
        }

        public void UnregisterListener(EventListener listener)
        {
            if (eventListeners.Contains(listener))
                eventListeners.Remove(listener);
        }
    }

}