using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpectralDaze.ScriptableObjects.Managers.SceneManager
{
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/Scene Manager/SceneInfo")]
    public class SceneInfo : ScriptableObject
    {
        //Not serializable
        //public Scene Scene;
        public string Scene;
        public List<SceneInfo> ConnectedScenes;
    }
}