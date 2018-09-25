using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SpectralDaze.Managers.AIManager
{
    /// <summary>
    /// A collection of AI tokens.
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject" />
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/AI Director/Token Collection")]
    public class TokenCollection : ScriptableObject
    {
        /// <summary>
        /// The amount of tokens for this type of token
        /// </summary>
        public int Amount;
        /// <summary>
        /// The list of tokens
        /// </summary>
        [HideInInspector]
        public List<Token> Tokens = new List<Token>();
        /// <summary>
        /// Generates the tokens.
        /// </summary>
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
