using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Code.AI;
using SpectralDaze.Managers.AIManager;
using UnityEngine;

namespace Managers
{
    public class AiDirector : MonoBehaviour
    {
        public enum TokenTypes { Shooting, Rushing }
        public Dictionary<TokenTypes, TokenCollection> TokenCollections = new Dictionary<TokenTypes, TokenCollection>();

        private void Start()
        {
            foreach(var value in Enum.GetValues(typeof(TokenTypes)).Cast<TokenTypes>())
            {
                TokenCollections.Add(value, Resources.Load<TokenCollection>("Managers/AIDirector/" + value.ToString() + "Tokens"));
                TokenCollections[value].GenerateTokens();
            }

            foreach (var tokenCollection in TokenCollections.Values)
            {
                foreach (var token in tokenCollection.Tokens)
                {
                    token.InUse = false;    
                    token.Requestor = null;
                    token.NoLongerInUse = false;
                }
            }
        }

        private void Update()
        {
            foreach (var tokenCollection in TokenCollections.Values)
            {
                foreach (var token in tokenCollection.Tokens)
                {
                    if (token.Requestor == null || token.NoLongerInUse)
                    {
                        token.Requestor = null;
                        token.InUse = false;
                        token.NoLongerInUse = false;
                    }
                    else if (token.Requestor != null && !token.InUse)
                    {
                        token.InUse = true;
                        token.Requestor.RequestForTokenAccepted(token);
                    }
                }
            }
        }
    }
}