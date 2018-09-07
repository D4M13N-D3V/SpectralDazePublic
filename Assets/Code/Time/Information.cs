using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.Time.Information
{
    [System.Serializable]
    public class Information
    {
        /// <summary>
        /// Normalized variable to use to modify the speed of the existing playback speed. Between 0-1
        /// </summary>
        public float AnimationModifier = 1;
        /// <summary>
        /// Normalized variable to use to modify the physics of the existing physics speed. Between 0-1
        /// </summary>
        public float PhysicsModifier = 1;
        /// <summary>
        /// Normalized variable to use to modify the movement of the existing movement speed. Between 0-1
        /// </summary>
        public float MovementModifier = 1;
    }
}