using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpectralDaze.ScriptableObjects.Managers.SceneManager
{
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/Scene Manager/RequiredScenes")]
    public class RequiredScenes : ScriptableObject
    {
        public List<SceneInfo> Scenes = new List<SceneInfo>();
    }
}