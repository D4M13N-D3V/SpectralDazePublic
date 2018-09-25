using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpectralDaze.AI;
using SpectralDaze.Player;
using UnityEngine;

namespace SpectralDaze.Etc
{
    /// <summary>
    /// Kills the player or enemy on touch.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class KillOnTouch : MonoBehaviour
    {
        /// <summary>
        /// Should this also kill enemy npcs
        /// </summary>
        public bool KillEnemys = true;
        void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.tag == "Player")
            {
                collision.collider.GetComponent<PlayerController>().EndGame();
            }
            else if (KillEnemys && collision.collider.tag == "Enemy")
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
