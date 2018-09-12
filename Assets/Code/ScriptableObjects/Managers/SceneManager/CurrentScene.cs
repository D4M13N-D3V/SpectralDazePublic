using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace SpectralDaze.ScriptableObjects.Managers.SceneManager
{
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/Scene Manager/CurrentScene")]
    public class CurrentScene : ScriptableObject
    {
        public SceneInfo SceneInfo;
    }
}