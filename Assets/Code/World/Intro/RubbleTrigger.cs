using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Event = SpectralDaze.Events.Event;

public class RubbleTrigger : MonoBehaviour
{
    public Event RubbleEvent;
    private void OnTriggerEnter(Collider col)
    {
        if (RubbleEvent == null)
        {
            Debug.LogError("No Event Provided To Rubble Trigger");
            return;
        }

        if (col.gameObject.tag == "Player")
        {
            RubbleEvent.Raise();
            Destroy(gameObject);
        }
    }
}
