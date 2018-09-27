using System.Collections;
using System.Collections.Generic;
using SpectralDaze.Managers.InputManager;
using TMPro;
using UnityEngine;

/// <summary>
/// Warns the player when bullet get sclose.
/// </summary>
public class IntroBulletWarner : MonoBehaviour
{
    /// <summary>
    /// The dash control
    /// </summary>
    public Control DashControl;
    /// <summary>
    /// The tutorial text
    /// </summary>
    private TextMeshProUGUI _tutorialText;
    /// <summary>
    /// Is this waiting for the button to continue?
    /// </summary>
    private bool waitingForButton = false;
    /// <summary>
    /// Time since script is started up.
    /// </summary>
    private float _lastTime;
    private void Start()
    {
        _lastTime = Time.realtimeSinceStartup;
        _tutorialText = GameObject.FindGameObjectWithTag("TutorialText").GetComponent<TextMeshProUGUI>();
        DashControl = Resources.Load<Control>("Managers/InputManager/Dash");
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            UnityEngine.Time.timeScale = 0.5f;
            _tutorialText.enabled = true;
            if (DashControl.IsMouseButton)
            {
                _tutorialText.text = "Press " + DashControl.MouseButton + " to dash!";
            }
            else
            {
                _tutorialText.text = "Press " + DashControl.KeyCode + " to dash!";
            }

            waitingForButton = true;
            StartCoroutine(WaitForDash());
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            waitingForButton = false;
            UnityEngine.Time.timeScale = 1;
            _tutorialText.enabled = false;
        }
    }

    /// <summary>
    /// Waits for the player to dash.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForDash()
    {
        while (!DashControl.JustPressed)
        {
            /*
             * rduce time scale
             */
            var myDeltaTime = UnityEngine.Time.realtimeSinceStartup - _lastTime;
            UnityEngine.Time.timeScale = Mathf.Lerp(Time.timeScale, 0, 0.05f*myDeltaTime);
            yield return new WaitForEndOfFrame();
        }
        waitingForButton = false;
        UnityEngine.Time.timeScale = 1;
        _tutorialText.enabled = false;
        Destroy(gameObject);
    }
}
