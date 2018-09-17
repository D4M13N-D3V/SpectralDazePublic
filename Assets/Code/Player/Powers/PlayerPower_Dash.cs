using SpectralDaze.Camera;
using SpectralDaze.DataTypes;
using SpectralDaze.Managers;
using SpectralDaze.Managers.AudioManager;
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
        public bool IsDashing = false;
        private ParticleSystem _particleSystem;
        private Vector3 _originalPos;
        private Vector3 _lastPos;
        public AudioClipInfo DashSound;
        public AudioQueue AudioQueue;
        private PlayerInfo _playerInfo;

        private ScriptableObjectQuartenion _inputRotation;

        private bool _clearPath = false;
        private float _realMaxDistance = 0;

        public override void Init(PlayerController pc)
        {
            Type = PowerTypes.Dash;
            _inputRotation = Resources.Load<ScriptableObjectQuartenion>("Player/InputRotation");
            AudioQueue = Resources.Load<AudioQueue>("Managers/Audio/AudioQueue");
            _playerInfo = Resources.Load<PlayerInfo>("Player/DefaultPlayerInfo");
            _particleSystem = Instantiate(ParticleSystem, pc.transform).GetComponent<ParticleSystem>();
            _particleSystem.transform.localPosition = Vector3.zero;
            _particleSystem.Stop();
        }


        public override void OnUpdate(PlayerController pc)
        {
            base.OnUpdate(pc);
            RaycastHit mouseHit;

            /*
            if (!Physics.Raycast(UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition), out mouseHit))
                if (!IsDashing)
                    return;
                    */

            if (!IsDashing && _particleSystem.isPlaying)
                StopDashing(pc);

            if (IsDashing)
            {
                var tgt = pc.transform.position + pc.transform.forward * DashSpeed * UnityEngine.Time.deltaTime;
                RaycastHit hit;
                NavMeshHit navHit;
                var nav = NavMesh.SamplePosition(tgt, out navHit, 1, NavMesh.AllAreas);
                var ray = Physics.Raycast(pc.transform.position, pc.transform.forward, out hit,
                    Vector3.Distance(pc.transform.position, tgt));

                if(ray && hit.collider.gameObject.layer == LayerMask.NameToLayer("Dashable"))
                    pc.transform.position = tgt;
                else if (ray &&  hit.collider.gameObject.tag =="BreakableWall")
                    pc.transform.position = tgt;
                else if (!ray)
                    pc.transform.position = tgt;
                else
                {
                    if (hit.collider.tag == "Movable")
                        hit.collider.gameObject.GetComponent<Movable>().Hit(pc.transform);
                    StopDashing(pc);
                }
            }

            _lastPos = pc.transform.position;

            if (IsDashing && Vector3.Distance(pc.transform.position, _originalPos) > _realMaxDistance)
            {
                StopDashing(pc);
            }

            if (!IsDashing && Control.JustPressed)
            {
                //If you want it to be based on posistion of mouse use this if not keep it commented
                //pc.transform.rotation = Quaternion.LookRotation(mouseHit.point - pc.transform.position);
                //pc.transform.eulerAngles = new Vector3(0, pc.transform.eulerAngles.y, 0);
                pc.transform.rotation = _inputRotation.Value;
                RaycastHit hit;
                NavMeshHit navHit;
                var navSamplePos = NavMesh.SamplePosition(pc.transform.position + pc.transform.forward * MaximumDashDistance, out navHit, 1, NavMesh.AllAreas);
                var raycast = Physics.Raycast(pc.transform.position, pc.transform.forward, out hit, Vector3.Distance(pc.transform.position, pc.transform.position + pc.transform.forward * MaximumDashDistance));
                if (navSamplePos && !raycast && navHit.distance < 1f)
                {
                    _realMaxDistance = MaximumDashDistance;
                }
                else
                {
                    _realMaxDistance = MaximumDashDistance - navHit.distance;
                }

                Debug.DrawLine(pc.transform.position, pc.transform.position + pc.transform.forward * MaximumDashDistance, Color.red);

                /*
                 * Breakable wall Code
                 */
                if (raycast && hit.collider.gameObject.tag == "BreakableWall")
                {
                    hit.collider.transform.GetChild(0).gameObject.SetActive(false);
                    hit.collider.transform.GetChild(1).gameObject.SetActive(true);
                    foreach (var rbody in hit.collider.transform.GetChild(1).GetComponentsInChildren<Rigidbody>())
                    {
                        rbody.velocity = pc.transform.forward*20;
                    }
                    _realMaxDistance = MaximumDashDistance;
                    hit.collider.enabled = false;
                }

                _particleSystem.Play();
                _originalPos = pc.transform.position;
                UnityEngine.Camera.main.gameObject.GetComponent<CameraFunctions>().Shake(0.05f, 0.2f);
                UnityEngine.Camera.main.gameObject.GetComponent<CameraFunctions>().FOVKick(2.0f, 0.2f);
                IsDashing = true;
                _playerInfo.CanMove = false;
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Dashable"), LayerMask.NameToLayer("Dasher"), true);
                pc.Agent.enabled = false;
                AudioQueue.Queue.Enqueue(DashSound);
            }
        }

        private void StopDashing(PlayerController pc)
        {
            _particleSystem.Stop();
            IsDashing = false;
            _playerInfo.CanMove = true;
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Dashable"), LayerMask.NameToLayer("Dasher"), false);
            pc.Agent.enabled = true;


            var navmeshObstacles = GameObject.FindObjectsOfType<NavMeshObstacle>();

            foreach (var obstacle in navmeshObstacles)
            {
                if (obstacle.gameObject.layer == LayerMask.NameToLayer("Dashable"))
                {
                    obstacle.enabled = true;
                }
            }
        }
    }
}