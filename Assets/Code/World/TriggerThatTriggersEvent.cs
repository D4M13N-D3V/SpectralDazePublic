using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event = SpectralDaze.Events.Event;

public class TriggerThatTriggersEvent : MonoBehaviour
{
    public Event RubbleEvent;
    private void OnTriggerEnter(Collider col)
    {
        if (RubbleEvent == null)
        {
            Debug.LogError("No Event Provided To Trigger");
            return;
        }

        if (col.gameObject.tag == "Player")
        {
            RubbleEvent.Raise();
            Destroy(gameObject);
        }
    }
}
