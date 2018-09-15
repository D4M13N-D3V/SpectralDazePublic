using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Spectral Daze/SO Variables/Quarternion")]
    public class ScriptableObjectQuartenion : ScriptableObject
    {
        public Quaternion Value;
    }
}