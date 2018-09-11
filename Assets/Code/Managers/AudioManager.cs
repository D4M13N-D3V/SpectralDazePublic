using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SpectralDaze.ScriptableObjects.Managers.Audio;

namespace SpectralDaze.Managers
{
    public class AudioManager : MonoBehaviour
    {
        public List<GameObject> AudioCache;
        public AudioQueue AudioQueue;
        private AudioSource _music1;
        private AudioSource _music2;

        private void Start()
        {
            AudioQueue = Resources.Load<AudioQueue>("Managers/Audio/AudioQueue");
            var m1 = new GameObject("Music Object ( NONE )");
            _music1 = m1.AddComponent<AudioSource>();
            var m2 = new GameObject("Music Object ( NONE )");
            _music2 = m2.AddComponent<AudioSource>();
        }

        private void Update()
        {
            while (AudioQueue.Queue.Any())
            {
                var clip = AudioQueue.Queue.Dequeue();
                PlaySfx(clip.Clip, clip.Loop, clip.Volume, clip.Duration);
            }
        }

        #region Music
        public void PlaySong(AudioClip song)
        {

        }
        #endregion

        #region SFX
        public int PlaySfx(AudioClip clip, bool loop, float volume=1, float duration = 0)
        {
            if (duration == 0)
                duration = clip.length;
            var obj = new GameObject("Sound Object ("+clip.name+" : "+clip.length+")");
            obj.transform.parent = transform;
            AudioCache.Add(obj);
            var audioComp = obj.AddComponent<AudioSource>();
            audioComp.spatialBlend = 0;
            audioComp.loop = loop;
            audioComp.clip = clip;
            audioComp.volume = volume;
            audioComp.Play();
            if(!loop)
                StartCoroutine(DestroyAfterTime(obj, duration));
            return AudioCache.FindIndex(x => x ==obj);
        }

        IEnumerator DestroyAfterTime(GameObject obj,float time)
        {
            yield return new WaitForSeconds(time);
            AudioCache.Remove(obj);
            Destroy(obj);
        }
        #endregion
    }
}