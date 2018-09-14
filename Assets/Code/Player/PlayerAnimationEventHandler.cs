using UnityEngine;
using System.Collections;

public class PlayerAnimationEventHandler : MonoBehaviour
{
    public AudioClip FootStep;
    public float MaxPitch = 1f;
    public float MinPitch = .5f;
    private AudioSource myAudioSource;

    public bool FinishedEmote;

    private void Start()
    {
        FinishedEmote = false;
        myAudioSource = GetComponent<AudioSource>();
    }

    public void FinishEmote()
    {
        FinishedEmote = true;
    }

    public void ResetEmoteState()
    {
        FinishedEmote = false;
    }

    public void PlayFootStepSFX()
    {
        myAudioSource.clip = FootStep;
        myAudioSource.pitch = Random.Range(MinPitch, MaxPitch);
        myAudioSource.Play();
    }
}
