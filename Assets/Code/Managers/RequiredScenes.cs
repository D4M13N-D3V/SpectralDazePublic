using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.Managers.SceneManager
{
    /// <summary>
    /// Scriptable object holding information about what scenes must always be loaded.
    /// </summary>
    /// <seealso cref="UnityEngine.ScriptableObject" />
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/Scene Manager/RequiredScenes")]
    public class RequiredScenes : ScriptableObject
    {
        /// <summary>
        /// A list of scenes required to be loaded at all times.
        /// </summary>
        public List<SceneInfo> Scenes = new List<SceneInfo>();
    }
}