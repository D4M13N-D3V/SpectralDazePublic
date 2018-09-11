using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpectralDaze.Camera;
using SpectralDaze.Etc;
using UnityEngine;

namespace Assets.Code.AI
{
    public class BaseAI : MonoBehaviour
    {
        public void Die()
        {
            UnityEngine.Camera.main.gameObject.GetComponent<CameraFunctions>().Shake(0.2f, 0.15f);
            Destroy(gameObject);
        }

        private void Update()
        {

        }
    }
}
