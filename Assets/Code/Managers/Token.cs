using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpectralDaze.AI;
using UnityEngine;

namespace SpectralDaze.Managers.AIManager
{
    /// <summary>
    /// The token.
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject" />
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/AI Director/Token")]
    public class Token : ScriptableObject
    {
        /// <summary>
        /// Is this token in use?
        /// </summary>
        public bool     InUse = false;
        /// <summary>
        /// Is this token no longer being used.
        /// </summary>
        public bool NoLongerInUse = false;
        /// <summary>
        /// The AI that requeste dthe token.
        /// </summary>
        public BaseAI Requestor;
    }
}
