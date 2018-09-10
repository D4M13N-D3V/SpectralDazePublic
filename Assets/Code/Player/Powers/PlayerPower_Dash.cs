using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SpectralDaze.Time;
using UnityEngine;
using UnityEngine.AI;

/*
 *
 *   DAMIENS VERSION WORKING WITH HIS NEW TIME SYSTEM!!
 *
 */
namespace SpectralDaze.Player
{
    [CreateAssetMenu(fileName = "Power_Dash", menuName = "Spectral Daze/PlayerPower/Dash")]
    public class PlayerPower_Dash : PlayerPower
    {
        public float DashSpeed = 0.1f;
        public float MaximumDashDistance;

        private bool _isDashing = false;

        public override void Init(PlayerController pc)
        {
        }

        public override void OnUpdate(PlayerController pc)
        {
            RaycastHit mouseHit;
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out mouseHit))
                return;
            NavMeshHit hit;

            var distBetweenMouseAndPlayer = Vector3.Distance(pc.transform.position, mouseHit.point);

            // Switch this out with something better.
            if (mouseHit.collider.gameObject.tag != "Walkable")
                return;

            Debug.Log(Vector3.Distance(mouseHit.point, pc.transform.position) +" "+ MaximumDashDistance);
            if (Vector3.Distance(mouseHit.point, pc.transform.position) > MaximumDashDistance)
                return;

            if (!NavMesh.SamplePosition(mouseHit.point, out hit, 1, NavMesh.AllAreas))
                return;

            if (!_isDashing && Input.GetMouseButtonDown(0))
            {
                _isDashing = true;
                LeanTween.move(pc.gameObject, hit.position+Vector3.up, DashSpeed * distBetweenMouseAndPlayer).setOnComplete(() =>
                {
                    _isDashing = false;;
                });
            }
        }
    }
}   