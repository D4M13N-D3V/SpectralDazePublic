using SpectralDaze.Camera;
using SpectralDaze.Player;
using UnityEngine;

namespace SpectralDaze.World
{
    /// <summary>
    /// A power pickup for the power system.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    [RequireComponent(typeof(BoxCollider))]
    public class PowerPickup : MonoBehaviour
    {
        /// <summary>
        /// The power to pickup
        /// </summary>
        public PlayerPower Power;
        /// <summary>
        /// The collider of this game object.
        /// </summary>
        private Collider _collider;
        
        private void Start()
        {
            _collider = GetComponent<BoxCollider>();
            _collider.enabled = true;
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.tag != "Player")
                return;
            Power.IsUnlocked = true;
            UnityEngine.Camera.main.GetComponent<CameraFunctions>().Shake(0.2f,0.1f);
            Destroy(gameObject);
        }
    }
}

