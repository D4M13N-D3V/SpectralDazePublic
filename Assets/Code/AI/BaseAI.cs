using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpectralDaze.Etc;
using UnityEngine;

namespace Assets.Code.AI
{
    public class BaseAI : MonoBehaviour
    {
        public void Die()
        {
            Destroy(gameObject);
        }

        private void Update()
        {

        }
    }
}
