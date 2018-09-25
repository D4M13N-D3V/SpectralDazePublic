using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SpectralDaze.Managers.AudioManager
{
    /// <summary>
    /// Manages the audio for the game
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class AudioManager : MonoBehaviour
    {
        /// <summary>
        /// The maximum amount of sounds that can be played at the same tiem
        /// </summary>
        public int MaximumAmountOfSounds = 40;
        /// <summary>
        /// The audio queue that is automatically loaded.
        /// </summary>
        private AudioQueue _audioQueue;
        private AudioSource _music1;
        private AudioSource _music2;
        /// <summary>
        /// A pool of audio sourcse that can be used to play sounds.
        /// </summary>
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

        /// <summary>
        /// Setups the pool of audio sources.
        /// </summary>
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
        /// <summary>
        /// Plays the SFX.
        /// </summary>
        /// <param name="clip">The clip.</param>
        /// <param name="loop">if set to <c>true</c> [loop].</param>
        /// <param name="volume">The volume.</param>
        /// <param name="duration">The duration.</param>
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

        /// <summary>
        /// Destroys the after time.
        /// </summary>
        /// <param name="audioSrc">The audio source.</param>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        IEnumerator DestroyAfterTime(AudioSource audioSrc,float time)
        {
            yield return new WaitForSeconds(time);
            audioSrc.gameObject.name = "Sound Object ( Unoccupied )";
            _audioPool.Enqueue(audioSrc);
        }
        #endregion
    }
}