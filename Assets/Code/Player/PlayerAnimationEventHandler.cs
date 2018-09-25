using UnityEngine;
using System.Collections;
using SpectralDaze.Managers.AudioManager;

namespace SpectralDaze.Player
{
    /// <summary>
    /// Handles the players animation events.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class PlayerAnimationEventHandler : MonoBehaviour
    {
        /// <summary>
        /// The maximum pitch
        /// </summary>
        public float MaxPitch = 1f;
        /// <summary>
        /// The minimum pitch
        /// </summary>
        public float MinPitch = .5f;

        /// <summary>
        /// Is emote finished
        /// </summary>
        public bool FinishedEmote;

        /// <summary>
        /// The footstep clip information
        /// </summary>
        public AudioClipInfo FootstepClipInfo;
        /// <summary>
        /// The audio queue
        /// </summary>
        public AudioQueue AudioQueue;

        private void Start()
        {
            AudioQueue = Resources.Load<AudioQueue>("Managers/Audio/AudioQueue");
            FootstepClipInfo = Resources.Load<AudioClipInfo>("Player/FootstepSound");
            FinishedEmote = false;
        }

        /// <summary>
        /// Called when emote is finished
        /// </summary>
        public void FinishEmote()
        {
            FinishedEmote = true;
        }

        /// <summary>
        /// Resets the state of the emote.
        /// </summary>
        public void ResetEmoteState()
        {
            FinishedEmote = false;
        }

        /// <summary>
        /// Plays the foot step SFX.
        /// </summary>
        public void PlayFootStepSFX()
        {
            AudioQueue.Queue.Enqueue(FootstepClipInfo);
        }
    }

}