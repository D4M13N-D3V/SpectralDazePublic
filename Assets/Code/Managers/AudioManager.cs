using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SpectralDaze.Managers.AudioManager
{
    public class AudioManager : MonoBehaviour
    {
        public int MaximumAmountOfSounds = 40;
        private AudioQueue _audioQueue;
        private AudioSource _music1;
        private AudioSource _music2;
        private Queue<AudioSource> _audioPool = new Queue<AudioSource>();


        private void Start()
        {
            _audioQueue = Resources.Load<AudioQueue>("Managers/Audio/AudioQueue");
            var m1 = new GameObject("Music Object ( NONE )");
            _music1 = m1.AddComponent<AudioSource>();
            m1.transform.parent = transform;
            var m2 = new GameObject("Music Object ( NONE )");
            _music2 = m2.AddComponent<AudioSource>();
            m2.transform.parent = transform;

            SetupPool();
        }

        private void Update()
        {
            while (_audioQueue.Queue.Any())
            {
                var clip = _audioQueue.Queue.Dequeue();
                PlaySfx(clip.Clip, clip.Loop, clip.Volume, clip.Duration);
            }
        }

        private void SetupPool()
        {
            for (int i = 0; i < MaximumAmountOfSounds; i++)
            {
                var obj = new GameObject("Sound Object ( Unoccupied )");
                obj.transform.parent = transform;
                _audioPool.Enqueue(obj.AddComponent<AudioSource>());
            }
        }

        #region Music
        public void PlaySong(AudioClip song)
        {

        }
        #endregion

        #region SFX
        public void PlaySfx(AudioClip clip, bool loop, float volume=1, float duration = 0)
        {
            if (duration == 0)
                duration = clip.length;

            var audioComp = _audioPool.Dequeue();
            audioComp.spatialBlend = 0;
            audioComp.loop = loop;
            audioComp.clip = clip;
            audioComp.volume = volume;
            audioComp.Play();
            audioComp.gameObject.name = "Sound Object ( " + audioComp.clip.name + ":" + audioComp.clip.length + " )";
            if (!loop)
                StartCoroutine(DestroyAfterTime(audioComp, duration));
        }

        IEnumerator DestroyAfterTime(AudioSource audioSrc,float time)
        {
            yield return new WaitForSeconds(time);
            audioSrc.gameObject.name = "Sound Object ( Unoccupied )";
            _audioPool.Enqueue(audioSrc);
        }
        #endregion
    }
}