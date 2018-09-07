using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{


    AudioSource SfxSource;

    // Use this for initialization
    void Start()
    {
        SfxSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySfx(AudioClip clip)
    {
        SfxSource.clip = clip;
        SfxSource.Play();
    }
}
