using System.Collections;
using System.Collections.Generic;
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
        public override void Init(PlayerController pc)
        {
            pc.timeInfo.Normal.MovementModifier = pc.playerInfo.MoveSpeed;
        }

        public override void OnUpdate(PlayerController pc)
        {
            if (!Input.GetMouseButton(0))
            {
                pc.playerInfo.MoveSpeed = pc.timeInfo.Normal.MovementModifier;
                pc.Animator.speed = pc.timeInfo.Normal.AnimationModifier;
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

                //NEW ADDITION IN THIS VERSION (Sets the players movement speed and animator speed back)
                pc.playerInfo.MoveSpeed = pc.timeInfo.Slowmotion.MovementModifier;
                pc.Animator.speed = pc.timeInfo.Slowmotion.AnimationModifier;
            }

            if (Input.GetMouseButtonUp(0))
            {
                // Move the player
                pc.playerInfo.MoveSpeed = pc.timeInfo.Normal.MovementModifier;
                pc.Animator.speed = pc.timeInfo.Normal.AnimationModifier;
                pc.transform.position = mouseHit.point + Vector3.up;
            }
        }
    }
}