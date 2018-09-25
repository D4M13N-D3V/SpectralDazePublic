using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.Camera
{
    public class CameraFunctions : MonoBehaviour
    {
        /// <summary>
        /// The original fov
        /// </summary>
        private float _originalFOV;

        private void Start()
        {
            _originalFOV = UnityEngine.Camera.main.fieldOfView;
        }           

        private void Update()
        {
            /*
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartCoroutine(FOVKick(2.0f, 0.2f));
            }
            */
        }

        /// <summary>
        /// Shakes the camera for specified duration and magnitude.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <param name="magnitude">The magnitude.</param>
        public void Shake(float duration, float magnitude)
        {
            StartCoroutine(shake(duration,magnitude));
        }

        IEnumerator shake(float duration, float magnitude)
        {
            Vector3 orignalPosistion = transform.localPosition;
            float elapsedTime = 0.0f;
            while (elapsedTime < duration)
            {
                float x = Random.Range(-1, 1) * magnitude;
                float z = Random.Range(-1, 1) * magnitude;
                transform.localPosition = orignalPosistion + new Vector3(x, orignalPosistion.y, z);
                elapsedTime += UnityEngine.Time.deltaTime;
                yield return null;
            }

            transform.localPosition = orignalPosistion;
        }

        /// <summary>
        /// Kicks the FOV for a camera effect.
        /// </summary>
        /// <param name="fovOffset">The fov offset.</param>
        /// <param name="time">The time it lasts.</param>
        public void FOVKick(float fovOffset, float time)
        {
            StartCoroutine(fovKick(fovOffset, time));
        }

        IEnumerator fovKick(float fovOffset, float time)
        {

            var t = 0.0f;
            var t2 = 0.0f;
            while (t < time / 2)
            {
                UnityEngine.Camera.main.fieldOfView = Mathf.Lerp(UnityEngine.Camera.main.fieldOfView,
                    UnityEngine.Camera.main.fieldOfView + fovOffset, t2);
                t += UnityEngine.Time.deltaTime;
                t2 += UnityEngine.Time.deltaTime / time;
                yield return new WaitForEndOfFrame();
            }
            t = 0;
            t2 = 0.0f;
            while (t < time / 2)
            {
                UnityEngine.Camera.main.fieldOfView = Mathf.Lerp(UnityEngine.Camera.main.fieldOfView,
                    _originalFOV, t2);
                t += UnityEngine.Time.deltaTime;
                t2 += UnityEngine.Time.deltaTime / time;
                yield return new WaitForEndOfFrame();
            }

            UnityEngine.Camera.main.fieldOfView = _originalFOV;
        }
    }
}
