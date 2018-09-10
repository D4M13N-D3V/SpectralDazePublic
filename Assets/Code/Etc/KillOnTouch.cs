using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code.AI;
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
            else if (collision.collider.tag == "Enemy")
            {
                foreach (var comp in collision.collider.gameObject.GetComponents(typeof(Component)))
                {
                    //SHouldnt have to use refelect GetComponent should work but isnt.
                    if (comp.GetType().IsSubclassOf(typeof(BaseAI))) { var enemy = (BaseAI)comp; enemy.Die(); }
                }
            }
        }
    }
}
