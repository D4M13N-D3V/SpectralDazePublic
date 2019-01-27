using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;
namespace SpectralDaze.Editor
{
    public class PlayerSceneTools
    {
        [MenuItem("SpectralDaze/ Add Player Scene")]
        private static void AddPlayerScene()
        {
            EditorSceneManager.OpenScene("Assets/Resources/Scenes/PlayerScene.unity", OpenSceneMode.Additive);
        }

        [MenuItem("SpectralDaze/ Add Player Spawn Point")]
        private static void AddSpawnPoint()
        {
            new GameObject("SPAWN_POINT");
        }
    }
}