using System.Collections;
using System.Collections.Generic;
using SpectralDaze.Managers.InputManager;
using SpectralDaze.Managers.PowerManager;
using TMPro;
using UnityEngine;

public class IntroBulletWarner : MonoBehaviour
{
    public Control DashControl;
    private TextMeshProUGUI _tutorialText;
    private bool waitingForButton = false;
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

    IEnumerator WaitForDash()
    {
        Debug.Log("TEST");
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
