using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpectralDaze.Player;
using UnityEngine;

namespace SpectralDaze.Etc
{
    public class KillOnTouch : MonoBehaviour
    {
        void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.tag == "Player")
            {
                collision.collider.GetComponent<PlayerController>().EndGame();
            }
        }
    }
}
