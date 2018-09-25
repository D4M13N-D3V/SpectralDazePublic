using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace SpectralDaze.Managers.SceneManager
{
    /// <summary>
    /// The currently loaded scene.
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject" />
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/Scene Manager/CurrentScene")]
    public class CurrentScene : ScriptableObject
    {
        /// <summary>
        /// The scene information of the current scene.
        /// </summary>
        public SceneInfo SceneInfo;
    }
}