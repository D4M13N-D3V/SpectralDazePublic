using UnityEngine;

namespace SpectralDaze.Managers
{
    /// <summary>
    /// Keeps the player from being destroyed on load.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    class DDOL : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Debug.Log("DDOL " + gameObject.name);
        }
    }
}