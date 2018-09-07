using UnityEngine;

namespace SpectralDaze.Managers
{
    class DDOL : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Debug.Log("DDOL " + gameObject.name);
        }
    }
}