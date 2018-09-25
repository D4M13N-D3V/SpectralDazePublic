using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Managers;
using Managers.AI;
using Sirenix.OdinInspector;
using SpectralDaze.Camera;
using SpectralDaze.Etc;
using SpectralDaze.Managers.AIManager;
using SpectralDaze.Managers.AudioManager;
using UnityEngine;

namespace SpectralDaze.AI
{
    public class BaseAI : MonoBehaviour
    {
        /// <summary>
        /// The audio queue
        /// </summary>
        public AudioQueue AudioQueue;
        /// <summary>
        /// The death sound
        /// </summary>
        public AudioClipInfo DeathSound;

        /// <summary>
        /// The current token for AI actions.
        /// </summary>
        public Token CurrentToken;

        /// <summary>
        /// Dictionary of tokens. Token types are looped through and scriptable objects are set as the value, with the type ask key.
        /// </summary>
        public Dictionary<AiDirector.TokenTypes, TokenCollection> TokenCollections = new Dictionary<AiDirector.TokenTypes, TokenCollection>();

        /// <summary>
        /// Setups this instance.
        /// </summary>
        public void Setup()
        {
            foreach (var value in Enum.GetValues(typeof(AiDirector.TokenTypes)).Cast<AiDirector.TokenTypes>())
            {
                TokenCollections.Add(value,Resources.Load<TokenCollection>("Managers/AIDirector/" + value.ToString() + "Tokens"));
            }
            GetAudioQueue();
        }

        /// <summary>
        /// Requests the token.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Returns false if no token available.</returns>
        internal bool RequestToken(AiDirector.TokenTypes type)
        {
            var token = GetAvailableTokenOfType(type);
            if (token == null) return false;
            Debug.Log(token);
            token.Requestor = this;
            return true;
        }

        /// <summary>
        /// Returns the token.
        /// </summary>
        internal void ReturnToken()
        {
            CurrentToken.NoLongerInUse = true;
        }

        /// <summary>
        /// Requests for token accepted.
        /// </summary>
        /// <param name="token">The token.</param>
        public void RequestForTokenAccepted(Token token)
        {
            CurrentToken = token;
        }

        /// <summary>
        /// Gets the type of the available token of.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Token, null if none are available</returns>
        private Token GetAvailableTokenOfType(AiDirector.TokenTypes type)
        {
            foreach (var token in TokenCollections[type].Tokens)
            {
                if (token.InUse == false)
                {
                    return token;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the audio queue.
        /// </summary>
        internal void GetAudioQueue()
        {
            AudioQueue = Resources.Load<AudioQueue>("Managers/Audio/AudioQueue");
        }

        /// <summary>
        /// Kills this AI.
        /// </summary>
        public void Die()
        {
            UnityEngine.Camera.main.gameObject.GetComponent<CameraFunctions>().Shake(0.2f, 0.15f);
            AudioQueue.Queue.Enqueue(DeathSound);
            Destroy(gameObject);
        }
    }
}
