using SpectralDaze.Camera;
using SpectralDaze.Managers.PowerManager;
using SpectralDaze.Player;
using UnityEngine;

namespace SpectralDaze.World
{
    [RequireComponent(typeof(BoxCollider))]
    public class PowerPickup : MonoBehaviour
    {
        public PlayerPower Power;
        private Collider _collider;

        private DashPower DashPower;
        private Power1 Power1;
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
    }
}

