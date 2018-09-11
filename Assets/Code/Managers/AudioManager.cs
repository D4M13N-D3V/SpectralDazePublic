using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SpectralDaze.Managers
{
    public class AudioManager : MonoBehaviour
    {
        public List<GameObject> AudioCache;

        public void PlaySfx(AudioClip clip, bool loop, float volume=1, float duration = 0)
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
            StartCoroutine(DestroyAfterTime(obj, duration));
        }

        IEnumerator DestroyAfterTime(GameObject obj,float time)
        {
            yield return new WaitForSeconds(time);
            AudioCache.Remove(obj);
            Destroy(obj);
        }
    }
}