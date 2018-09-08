using System.Collections;
using System.Collections.Generic;
using SpectralDaze.Time;
using UnityEngine;

public class TimeBubbleController : MonoBehaviour
{
    public Manipulations Type = Manipulations.Slow;

    private void Start()
    {
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, new Vector3(7, 7, 7), 0.5f);
    }

    public void SelfDestruct()
    {
        LeanTween.scale(gameObject, Vector3.zero, 0.5f).setOnComplete(() =>
        {
            Destroy(gameObject);
        });
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Time"))
        {
            other.gameObject.SendMessage("StartTimeManipulation", (int)Type);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Time"))
        {
            other.gameObject.SendMessage("StopTimeManipulation");
        }
    }
}
