using SpectralDaze.Camera;
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
        public GameObject ParticleSystem;
        private bool _isDashing = false;
        private ParticleSystem _particleSystem;
        private Vector3 _originalPos;
        private Vector3 _lastPos;

        public override void Init(PlayerController pc)
        {
            _particleSystem = Instantiate(ParticleSystem, pc.transform).GetComponent<ParticleSystem>();
            _particleSystem.transform.localPosition = Vector3.zero;
            _particleSystem.Stop();
        }

        public override void OnUpdate(PlayerController pc)
        {
            RaycastHit mouseHit;
            if (!Physics.Raycast(UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition), out mouseHit))
                return;

            /*
            //NavMeshHit hit;
            //NavMeshPath path = new NavMeshPath(); ;

            //var distBetweenMouseAndPlayer = Vector3.Distance(pc.transform.position, mouseHit.point);

            // Switch this out with something better.
            //if (mouseHit.collider.gameObject.tag != "Walkable")
            //    return;
            
            //if (Vector3.Distance(mouseHit.point, pc.transform.position) > MaximumDashDistance)
            //    return;
            
            //if (!NavMesh.SamplePosition(mouseHit.point, out hit, 1, NavMesh.AllAreas))
            //    return;

            //pc.Agent.CalculatePath(hit.position, path);
            //if(path.status != NavMeshPathStatus.PathComplete)
            //    return;; */

            if (_isDashing)
            {Debug.Log(pc.transform.position + pc.transform.forward * DashSpeed * UnityEngine.Time.deltaTime);
                pc.transform.position = pc.transform.position + pc.transform.forward * DashSpeed* UnityEngine.Time.deltaTime;
            }

            if (_isDashing && System.Math.Round(_lastPos.x, 1) == System.Math.Round(pc.transform.position.x, 1)
                           && System.Math.Round(_lastPos.y, 1) == System.Math.Round(pc.transform.position.y, 1)
                           && System.Math.Round(_lastPos.z, 1) == System.Math.Round(pc.transform.position.z, 1)
                           || Vector3.Distance(pc.transform.position, _originalPos) > MaximumDashDistance)
            {
                _particleSystem.Stop();
                _isDashing = false;
            }
            _lastPos = pc.transform.position;

            if (!_isDashing && Input.GetMouseButtonDown(0))
            {
                pc.transform.rotation = Quaternion.LookRotation(mouseHit.point - pc.transform.position);
                pc.transform.eulerAngles = new Vector3(0, pc.transform.eulerAngles.y, 0);
                _particleSystem.Play();
                _originalPos = pc.transform.position;
                UnityEngine.Camera.main.gameObject.GetComponent<CameraFunctions>().Shake(0.05f, 0.2f);
                _isDashing = true;
            }
        }
    }
}