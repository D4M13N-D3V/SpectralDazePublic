using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SpectralDaze.Managers.SceneManager;
/// <summary>
/// When a gameobject enters the trigger of this, set the current scene for the scenemanager.
/// </summary>
public class SceneTrigger : MonoBehaviour
{
    /// <summary>
    /// The scenes folder
    /// </summary>
    public string SceneFolder;
    /// <summary>
    /// The scene information scriptable object
    /// </summary>
    public SceneInfo SceneInfo;
    /// <summary>
    /// The current scene scriptable object
    /// </summary>
    private CurrentScene CurrentScene;
    private void Start()
    {
        CurrentScene = Resources.Load<CurrentScene>("Managers/SceneManager/CurrentScene");
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            CurrentScene.SceneInfo = SceneInfo;
        }
    }
}
