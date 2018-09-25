using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SpectralDaze.Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace SpectralDaze.Managers.SceneManager
{
    /// <summary>
    /// Manages the scenes for the game
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class SceneManager : MonoBehaviour
    {
        /// <summary>
        /// The scenes currently loaded in the game
        /// </summary>
        public List<SceneInfo> LoadedScenes = new List<SceneInfo>();
        /// <summary>
        /// The main current scene that is loaded
        /// </summary>
        public CurrentScene CurrentScene;
        /// <summary>
        /// The default scene
        /// </summary>
        public DefaultScene DefaultScene;
        /// <summary>
        /// The required scenes to be loaded
        /// </summary>
        public RequiredScenes RequiredScenes;
        /// <summary>
        /// The nav mesh surface to use to regenerate navmesh.
        /// </summary>
        private NavMeshSurface _surface;
        /// <summary>
        /// Has the nav mesh generated
        /// </summary>
        private bool navMeshGenerated = false;

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <summary>
        /// Loads the specified scene.
        /// </summary>
        /// <param name="loadingScene">The scene to be loaded.</param>
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