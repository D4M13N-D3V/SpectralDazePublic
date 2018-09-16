using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code.AI;
using UnityEngine;

namespace SpectralDaze.Managers.AIManager
{
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/AI Director/Token Collection")]
    public class TokenCollection : ScriptableObject
    {
        public int Amount;
        [HideInInspector]
        public List<Token> Tokens = new List<Token>();
        public void GenerateTokens()
        {
            Tokens.Clear();
            for (int i = 0; i < Amount; i++)
            {
                Tokens.Add(CreateInstance("Token") as Token);
            }
        }
    }
}
