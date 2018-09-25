using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace SpectralDaze.Managers.SceneManager
{
    /// <summary>
    /// The default scene to load.
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject" />
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/Scene Manager/DefaultScene")]
    public class DefaultScene : ScriptableObject
    {
        /// <summary>
        /// The scene information of the scene to load
        /// </summary>
        public SceneInfo SceneInfo;
    }
}