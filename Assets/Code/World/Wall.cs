using System.Collections;
using System.Collections.Generic;
using SpectralDaze.Player;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public virtual void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            var pc = collider.gameObject.GetComponent<PlayerPowerController>();
            pc.DashPower.IsDashing = false;
        }
    }

    public virtual void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            var pc = collider.gameObject.GetComponent<PlayerPowerController>();
            pc.DashPower.IsDashing = false;
            Debug.Log("TEST");
        }
    }

}

