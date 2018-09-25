using UnityEngine;

namespace SpectralDaze.Player
{
    /// <summary>
    /// The player information
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject" />
    [CreateAssetMenu(menuName = "Spectral Daze/Player/Player Info")]
    public class PlayerInfo : ScriptableObject
    {
        /// <summary>
        /// The movement speed
        /// </summary>
        public float MoveSpeed = 2f;
        /// <summary>
        /// Can the player move
        /// </summary>
        public bool CanMove = true;
    }
}
