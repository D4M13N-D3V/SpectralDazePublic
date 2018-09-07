using UnityEngine;

namespace SpectralDaze.ScriptableObjects.Stats
{
    [CreateAssetMenu(menuName = "Spectral Daze/Player/Player Info")]
    public class PlayerInfo : ScriptableObject
    {
        public float MoveSpeed = 2f;
        public bool CanMove = true;
    }
}
