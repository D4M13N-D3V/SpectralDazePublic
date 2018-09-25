using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpectralDaze.Managers.SceneManager
{
    /// <summary>
    /// Scene information for the scene manager
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject" />
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/Scene Manager/SceneInfo")]
    public class SceneInfo : ScriptableObject
    {
        /// <summary>
        /// The scene name.
        /// </summary>
        public string Scene;
        /// <summary>
        /// The scenes that also are loaded when this scene is loaded.
        /// </summary>
        public List<SceneInfo> ConnectedScenes;
    }
}