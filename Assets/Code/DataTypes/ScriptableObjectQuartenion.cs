using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.DataTypes
{
    /// <summary>
    /// Scriptable object for holdinga persistant Quarternion value.
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject" />
    [CreateAssetMenu(menuName = "Spectral Daze/SO Variables/Quarternion")]
    public class ScriptableObjectQuartenion : ScriptableObject
    {
        /// <summary>
        /// The value
        /// </summary>
        public Quaternion Value;
    }
}