using System.Collections;
using System.Collections.Generic;
using SpectralDaze.Player;
using UnityEngine;

public class Wall : MonoBehaviour
{
    internal void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            var pc = collider.gameObject.GetComponent<PlayerPowerController>();
            pc.DashPower.IsDashing = false;
        }
    }

    internal void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            var pc = collider.gameObject.GetComponent<PlayerPowerController>();
            pc.DashPower.IsDashing = false;
        }
    }

}

