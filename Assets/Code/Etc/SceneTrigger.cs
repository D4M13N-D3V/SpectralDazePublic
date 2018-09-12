using System.Collections;
using System.Collections.Generic;
using SpectralDaze.ScriptableObjects.Managers.SceneManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTrigger : MonoBehaviour
{
    public string SceneFolder;
    public SceneInfo SceneInfo;
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
