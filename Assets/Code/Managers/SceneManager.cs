using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SpectralDaze.Player;
using SpectralDaze.ScriptableObjects.Managers.SceneManager;
using UnityEditor;
using UnityEditor.AI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace SpectralDaze.Managers
{
    public class SceneManager : MonoBehaviour
    {
        public List<SceneInfo> LoadedScenes = new List<SceneInfo>();
        public CurrentScene CurrentScene;
        public DefaultScene DefaultScene;
        public RequiredScenes RequiredScenes;
        private  NavMeshSurface _surface;
        private bool navMeshGenerated = false;
        private void Start()
        {
            _surface = GetComponent<NavMeshSurface>();

            CurrentScene = Resources.Load<CurrentScene>("Managers/SceneManager/CurrentScene");
            DefaultScene = Resources.Load<DefaultScene>("Managers/SceneManager/DefaultScene");
            RequiredScenes = Resources.Load<RequiredScenes>("Managers/SceneManager/RequiredScenes");

            foreach (var scene in RequiredScenes.Scenes)
            {
                LoadedScenes.Add(scene);
            }   
        }
        private void Update()
        {
            if (navMeshGenerated == false)
            {
                var loaded = false;
                foreach (var scene in LoadedScenes)
                {
                    loaded = UnityEngine.SceneManagement.SceneManager.GetSceneByName(scene.Scene).isLoaded;
                    if (loaded == false)
                    {
                        break;
                    }
                }

                if (loaded)
                {
                    _surface.BuildNavMesh();
                    navMeshGenerated = true;
                }
            }
            if (LoadedScenes.Any() && LoadedScenes.Count==RequiredScenes.Scenes.Count)
            {
                CurrentScene.SceneInfo = DefaultScene.SceneInfo;
                LoadScene(CurrentScene.SceneInfo);
                return;
            }

            if (LoadedScenes[RequiredScenes.Scenes.Count] != CurrentScene.SceneInfo)
            {
                LoadScene(CurrentScene.SceneInfo);
            }
        }

        public void LoadScene(SceneInfo loadingScene)
        {
            var scenesToRemove = new List<SceneInfo>();
            foreach (var loadedScene in LoadedScenes)
            {
                if (loadingScene==loadedScene)
                    continue;
                if (RequiredScenes.Scenes.Contains(loadedScene))
                    continue;
                if (loadingScene.ConnectedScenes.Contains(loadedScene))
                    continue;
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(loadedScene.Scene);
                scenesToRemove.Add(loadedScene);
            }

            foreach (var sceneToRemove in scenesToRemove)
            {
                LoadedScenes.Remove(sceneToRemove);
            }

            foreach (var connectedScene in loadingScene.ConnectedScenes)
            {
                if (!LoadedScenes.Contains(connectedScene))
                {
                    LoadedScenes.Add(connectedScene);
                    UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(connectedScene.Scene,LoadSceneMode.Additive);
                }
            }

            if (!LoadedScenes.Contains(loadingScene))
            {
                LoadedScenes.Add(loadingScene);
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(loadingScene.Scene, LoadSceneMode.Additive);
            }

            //make sure that the mains scene is in right spot.
            var oldElement1Object = LoadedScenes[RequiredScenes.Scenes.Count];
            var oldElement2Index = LoadedScenes.FindIndex(x => x == loadingScene);
            LoadedScenes[RequiredScenes.Scenes.Count] = loadingScene;
            LoadedScenes[oldElement2Index] = oldElement1Object;

            navMeshGenerated = false;
        }

    }
        
}