using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.Camera
{
    public class CameraFunctions : MonoBehaviour
    {
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
                Debug.Log(elapsedTime);
                float x = Random.Range(-1, 1) * magnitude;
                float z = Random.Range(-1, 1) * magnitude;
                transform.localPosition = orignalPosistion + new Vector3(x, orignalPosistion.y, z);
                elapsedTime += UnityEngine.Time.deltaTime;
                yield return null;
            }

            transform.localPosition = orignalPosistion;
        }
    }
}
