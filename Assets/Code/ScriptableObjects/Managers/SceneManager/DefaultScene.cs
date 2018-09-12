using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace SpectralDaze.ScriptableObjects.Managers.SceneManager
{
    [CreateAssetMenu(menuName = "Spectral Daze/Managers/Scene Manager/DefaultScene")]
    public class DefaultScene : ScriptableObject
    {
        public SceneInfo SceneInfo;
    }
}