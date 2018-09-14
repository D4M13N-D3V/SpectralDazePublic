using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Managers;
using SpectralDaze.Camera;
using SpectralDaze.Etc;
using SpectralDaze.ScriptableObjects.AIManager;
using SpectralDaze.ScriptableObjects.Managers.Audio;
using UnityEngine;

namespace Assets.Code.AI
{
    public class BaseAI : MonoBehaviour
    {
        public AudioQueue AudioQueue;
        public AudioClipInfo DeathSound;

        public Token CurrentToken;

        public Dictionary<AiDirector.TokenTypes, TokenCollection> TokenCollections = new Dictionary<AiDirector.TokenTypes, TokenCollection>();

        public void Setup()
        {
            foreach (var value in Enum.GetValues(typeof(AiDirector.TokenTypes)).Cast<AiDirector.TokenTypes>())
            {
                TokenCollections.Add(value,Resources.Load<TokenCollection>("Managers/AIDirector/" + value.ToString() + "Tokens"));
            }
            GetAudioQueue();
        }   

        internal bool RequestToken(AiDirector.TokenTypes type)
        {
            var token = GetAvailableTokenOfType(type);
            if (token == null) return false;
            Debug.Log(token);
            token.Requestor = this;
            return true;
        }
            
        internal void ReturnToken()
        {
            CurrentToken.NoLongerInUse = true;
        }

        public void RequestForTokenAccepted(Token token)
        {
            CurrentToken = token;
        }


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

        internal void GetAudioQueue()
        {
            AudioQueue = Resources.Load<AudioQueue>("Managers/Audio/AudioQueue");
        }

        public void Die()
        {
            UnityEngine.Camera.main.gameObject.GetComponent<CameraFunctions>().Shake(0.2f, 0.15f);
            AudioQueue.Queue.Enqueue(DeathSound);
            Destroy(gameObject);
        }
    }
}
