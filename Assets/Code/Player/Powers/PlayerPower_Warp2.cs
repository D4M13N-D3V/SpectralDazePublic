using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SpectralDaze.Time;
using UnityEngine;

namespace SpectralDaze.Player
{
    /// <summary>
    /// Warp power for players
    /// </summary>
    /// <seealso cref="SpectralDaze.Player.PlayerPower" />
    [CreateAssetMenu(fileName = "Power_Warp_2", menuName = "Spectral Daze/PlayerPower/Warp2")]
    public class PlayerPower_Warp2 : PlayerPower
    {
        /// <summary>
        /// The time bubble prefab
        /// </summary>
        public GameObject TimeBubblePrefab;
        /// <summary>
        /// The size to put the time bubble up to.
        /// </summary>
        public Vector3 BubbleScale = new Vector3(7,7,7);

        /// <summary>
        /// The time bubble that has been created 
        /// </summary>
        private GameObject _timeBubble;

        /// <inheritdoc />
        public override void Init(PlayerController pc)
        {
        }

        /// <inheritdoc />
        public override void OnUpdate(PlayerController pc)
        {
            if (!Control.BeingPressed)
            {
                if (_timeBubble != null)
                    _timeBubble.GetComponent<TimeBubbleController>().SelfDestruct();
            }

            RaycastHit mouseHit;
            if (!Physics.Raycast(UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition), out mouseHit))
                return;

            // Switch this out with something better.
            if (mouseHit.collider.gameObject.tag != "Walkable")
                return;

            Vector3 adjustedHitPoint = mouseHit.point + Vector3.up;
            if (Vector3.Distance(adjustedHitPoint, pc.transform.position) > 7.5f)
                return;

            if (Control.BeingPressed)
            {
                // If they're holding right click
                mouseHit.point.DrawDebug(Color.red, 0.1f, 0.25f);

                if (_timeBubble == null)
                {
                    _timeBubble = Instantiate(TimeBubblePrefab, pc.transform);
                    _timeBubble.GetComponent<TimeBubbleController>().BubbleScale = BubbleScale;
                    _timeBubble.transform.localPosition = new Vector3(0,-1,0);
                    _timeBubble.GetComponent<TimeBubbleController>().Type = Manipulations.Slow;
                    _timeBubble.name = "TimeBubble";
                }
            }

            if (Control.JustReleased)
            {
                pc.transform.position = mouseHit.point + Vector3.up;
                _timeBubble.GetComponent<TimeBubbleController>().SelfDestruct();    
            }
        }
    }
}