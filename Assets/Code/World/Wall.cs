using System.Collections;
using System.Collections.Generic;
using SpectralDaze.Player;
using SpectralDaze.ScriptableObjects.Managers.PowerManager;
using UnityEngine;

namespace SpectralDaze.World
{
    public class Wall : MonoBehaviour
    {
        public DashPower DashPower;

        internal virtual void Start()
        {
            DashPower = Resources.Load<DashPower>("Managers/PowerManager/DashPower");
        }

        internal void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag == "Player")
            {
                DashPower.Power.IsDashing = false;
            }
        }

        internal void OnTriggerStay(Collider collider)
        {
            if (collider.gameObject.tag == "Player")
            {
                DashPower.Power.IsDashing = false;
            }
        }

    }

}