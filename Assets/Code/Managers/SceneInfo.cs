using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpectralDaze.Managers.SceneManager
{
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/Scene Manager/SceneInfo")]
    public class SceneInfo : ScriptableObject
    {
        public string Scene;
        public List<SceneInfo> ConnectedScenes;
    }
}