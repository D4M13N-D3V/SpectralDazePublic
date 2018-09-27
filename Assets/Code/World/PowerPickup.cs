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
		/*
        /// <summary>
        /// The power to pickup
        /// </summary>
        public PlayerPower Power;
        /// <summary>
        /// The collider of this game object.
        /// </summary>
        private Collider _collider;

        /// <summary>
        /// The dash power scriptable object for SO Architecture
        /// </summary>
        private DashPower DashPower;
        /// <summary>
        /// The power1 scriptable object for SO Architecture
        /// </summary>
        private Power1 Power1;
        /// <summary>
        /// The power2 scriptable object for SO Architecture
        /// </summary>
        private Power2 Power2;

        private void Start()
        {
            _collider = GetComponent<BoxCollider>();
            _collider.enabled = true;
            _collider.isTrigger = true;

            Power1 = Resources.Load<Power1>("Managers/PowerManager/Power1");
            Power2 = Resources.Load<Power2>("Managers/PowerManager/Power2");
            DashPower = Resources.Load<DashPower>("Managers/PowerManager/DashPower");
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.tag != "Player")
                return;
                    
            switch (Power.Type)
            {
                case PowerTypes.Dash:
                    if(Power is PlayerPower_Dash)
                        DashPower.Power = (PlayerPower_Dash)Power;
                    break;
                case PowerTypes.Power:
                    if (Power1.Power == null)
                        Power1.Power = Power;
                    if (Power2.Power == null)
                        Power2.Power = Power;
                    break;
            }
            UnityEngine.Camera.main.GetComponent<CameraFunctions>().Shake(0.2f,0.1f);
            Destroy(gameObject);
        }
		*/
    }
}

