using SpectralDaze.Camera;
using SpectralDaze.Managers;
using SpectralDaze.ScriptableObjects.Managers.Audio;
using SpectralDaze.ScriptableObjects.Managers.InputManager;
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
        //public float MaximumDashTime = 0.5f;
        public float MaximumDashDistance;
        public GameObject ParticleSystem;
        public bool IsDashing = false;
        private ParticleSystem _particleSystem;
        private Vector3 _originalPos;
        private Vector3 _lastPos;
        private bool _clearPath = false;
        public AudioClipInfo DashSound;
        public AudioQueue AudioQueue;
        //private float _duration;

        private PlayerInfo _playerInfo;

        public override void Init(PlayerController pc)
        {
            Debug.Log("TEST");
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
            if (!Physics.Raycast(UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition), out mouseHit))
                return;

            if(!IsDashing && _particleSystem.isPlaying)
                StopDashing(pc);
            
            if (IsDashing)
            {
                var tgt = pc.transform.position + pc.transform.forward * DashSpeed * UnityEngine.Time.deltaTime;
                RaycastHit hit;

                if (!Physics.Raycast(pc.transform.position, pc.transform.forward, out hit,
                    Vector3.Distance(pc.transform.position, tgt)))
                    pc.transform.position = tgt;
                else
                    StopDashing(pc);
            }

            _lastPos = pc.transform.position;
            
            if (IsDashing && Vector3.Distance(pc.transform.position, _originalPos) > MaximumDashDistance)
            {
                StopDashing(pc);
            }

            if (!IsDashing && Control.JustPressed)
            {
                pc.transform.rotation = Quaternion.LookRotation(mouseHit.point - pc.transform.position);
                pc.transform.eulerAngles = new Vector3(0, pc.transform.eulerAngles.y, 0);
                _particleSystem.Play();
                _originalPos = pc.transform.position;
                UnityEngine.Camera.main.gameObject.GetComponent<CameraFunctions>().Shake(0.05f, 0.2f);
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
        }
    }
}