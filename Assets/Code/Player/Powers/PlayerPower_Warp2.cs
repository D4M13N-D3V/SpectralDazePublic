using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SpectralDaze.Time;
using UnityEngine;
/*
 *
 *   DAMIENS VERSION WORKING WITH HIS NEW TIME SYSTEM!!
 *
 */
namespace SpectralDaze.Player
{
    [CreateAssetMenu(fileName = "Power_Warp_2", menuName = "Spectral Daze/PlayerPower/Warp2")]
    public class PlayerPower_Warp2 : PlayerPower
    {
        public GameObject TimeBubblePrefab;
        public Vector3 BubbleScale = new Vector3(7,7,7);

        private GameObject _timeBubble;

        public override void Init(PlayerController pc)
        {
            pc.timeInfo.Data.SingleOrDefault(x => x.Type == Manipulations.Normal).Stats.MovementModifier = pc.PlayerInfo.MoveSpeed;
        }

        public override void OnUpdate(PlayerController pc)
        {
            if (!Input.GetMouseButton(0))
            {
                pc.PlayerInfo.MoveSpeed = pc.timeInfo.Data.SingleOrDefault(x=>x.Type==Manipulations.Normal).Stats.MovementModifier;
                pc.Animator.speed = pc.timeInfo.Data.SingleOrDefault(x => x.Type == Manipulations.Normal).Stats.AnimationModifier;
                if (_timeBubble != null)
                    _timeBubble.GetComponent<TimeBubbleController>().SelfDestruct();
            }

            RaycastHit mouseHit;
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mouseHit))
                return;

            // Switch this out with something better.
            if (mouseHit.collider.gameObject.tag != "Walkable")
                return;

            Vector3 adjustedHitPoint = mouseHit.point + Vector3.up;
            if (Vector3.Distance(adjustedHitPoint, pc.transform.position) > 12f)
                return;

            if (Input.GetMouseButton(0))
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

                //NEW ADDITION IN THIS VERSION (Sets the players movement speed and animator speed back)
                pc.PlayerInfo.MoveSpeed = pc.timeInfo.Data.SingleOrDefault(x => x.Type == Manipulations.Slow).Stats.MovementModifier;
                pc.Animator.speed = pc.timeInfo.Data.SingleOrDefault(x => x.Type == Manipulations.Slow).Stats.AnimationModifier;
            }

            if (Input.GetMouseButtonUp(0))
            {
                // Move the player
                pc.PlayerInfo.MoveSpeed = pc.timeInfo.Data.SingleOrDefault(x => x.Type == Manipulations.Normal).Stats.MovementModifier;
                pc.Animator.speed = pc.timeInfo.Data.SingleOrDefault(x => x.Type == Manipulations.Normal).Stats.AnimationModifier;
                pc.transform.position = mouseHit.point + Vector3.up;
                _timeBubble.GetComponent<TimeBubbleController>().SelfDestruct();    
            }
        }
    }
}