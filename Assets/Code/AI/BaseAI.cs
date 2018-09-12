using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpectralDaze.Camera;
using SpectralDaze.Etc;
using SpectralDaze.ScriptableObjects.Managers.Audio;
using UnityEngine;

namespace Assets.Code.AI
{
    public class BaseAI : MonoBehaviour
    {
        public AudioQueue AudioQueue;
        public AudioClipInfo DeathSound;

        private void Start()
        {
            GetAudioQueue();
        }

        internal void GetAudioQueue()
        {
            AudioQueue = Resources.Load<AudioQueue>("Managers/Audio/AudioQueue");
        }

        private void Update()
        {

        }

        public void Die()
        {
            UnityEngine.Camera.main.gameObject.GetComponent<CameraFunctions>().Shake(0.2f, 0.15f);
            AudioQueue.Queue.Enqueue(DeathSound);
            Destroy(gameObject);
        }
    }
}
