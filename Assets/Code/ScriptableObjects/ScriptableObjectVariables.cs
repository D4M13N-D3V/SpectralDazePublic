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

    [CreateAssetMenu(menuName = "Spectral Daze/SO Variables/Vector3")]
    public class ScriptableObjectVector3 : ScriptableObject
    {
        public Vector3 Value;
    }

    [CreateAssetMenu(menuName = "Spectral Daze/SO Variables/String")]
    public class ScriptableObjectString : ScriptableObject
    {
        public string Value;
    }

    [CreateAssetMenu(menuName = "Spectral Daze/SO Variables/Float")]
    public class ScriptableObjectFloat : ScriptableObject
    {
        public float Value;
    }

    [CreateAssetMenu(menuName = "Spectral Daze/SO Variables/Int")]
    public class ScriptableObjectInt : ScriptableObject
    {
        public int Value;
    }

}