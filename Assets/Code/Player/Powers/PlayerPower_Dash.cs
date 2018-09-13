using SpectralDaze.Camera;
using SpectralDaze.Managers;
using SpectralDaze.ScriptableObjects.Managers.Audio;
using SpectralDaze.ScriptableObjects.Stats;
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
        public float MaximumDashTime = 0.5f;
        public float MaximumDashDistance;
        public GameObject ParticleSystem;
        public bool IsDashing = false;
        private ParticleSystem _particleSystem;
        private Vector3 _originalPos;
        private Vector3 _lastPos;
        public AudioClipInfo DashSound;
        public AudioQueue AudioQueue;
        private float _duration;

        private PlayerInfo _playerInfo;

        public override void Init(PlayerController pc)
        {
            AudioQueue = Resources.Load<AudioQueue>("Managers/Audio/AudioQueue");
            _playerInfo = Resources.Load<PlayerInfo>("Player/DefaultPlayerInfo");
            _particleSystem = Instantiate(ParticleSystem, pc.transform).GetComponent<ParticleSystem>();
            _particleSystem.transform.localPosition = Vector3.zero;
            _particleSystem.Stop();
        }


        public override void OnUpdate(PlayerController pc)
        {
            RaycastHit mouseHit;
            if (!Physics.Raycast(UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition), out mouseHit))
                return;

            if(!IsDashing)
                StopDashing(pc);

            if (IsDashing)
            {
                _duration += UnityEngine.Time.deltaTime;
                if (_duration >= MaximumDashTime)
                {
                    StopDashing(pc);
                }
            }

            if (IsDashing)
            {
                pc.transform.position = pc.transform.position + pc.transform.forward * DashSpeed * UnityEngine.Time.deltaTime;
            }

            if (IsDashing && System.Math.Round(_lastPos.x, 1) == System.Math.Round(pc.transform.position.x, 1)
                           && System.Math.Round(_lastPos.y, 1) == System.Math.Round(pc.transform.position.y, 1)
                           && System.Math.Round(_lastPos.z, 1) == System.Math.Round(pc.transform.position.z, 1)
                           || Vector3.Distance(pc.transform.position, _originalPos) > MaximumDashDistance)
            {
                StopDashing(pc);
            }
            _lastPos = pc.transform.position;

            if (!IsDashing && Input.GetMouseButtonDown(0))
            {
                pc.transform.rotation = Quaternion.LookRotation(mouseHit.point - pc.transform.position);
                pc.transform.eulerAngles = new Vector3(0, pc.transform.eulerAngles.y, 0);
                _particleSystem.Play();
                _originalPos = pc.transform.position;
                _duration = 0;
                UnityEngine.Camera.main.gameObject.GetComponent<CameraFunctions>().Shake(0.05f, 0.2f);
                IsDashing = true;
                _playerInfo.CanMove = false;
                Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Dashable"), LayerMask.NameToLayer("Dasher"),true);
                pc.Agent.enabled = false;
                AudioQueue.Queue.Enqueue(DashSound);
            }
        }

        private void StopDashing(PlayerController pc)
        {
            _particleSystem.Stop();
            IsDashing = false;
            _playerInfo.CanMove = true;
            //pc.Animator.SetBool("IsDashing", false);
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Dashable"), LayerMask.NameToLayer("Dasher"), false);
            pc.Agent.enabled = true;
        }
    }
}