using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code.AI;
using UnityEngine;

namespace SpectralDaze.ScriptableObjects.AIManager
{
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/AI Director/Token Collection")]
    public class TokenCollection : ScriptableObject
    {
        public List<Token> Tokens = new List<Token>();
    }
}
