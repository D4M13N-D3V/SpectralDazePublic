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
    private void Start()
    {
        _tutorialText = GameObject.FindGameObjectWithTag("TutorialText").GetComponent<TextMeshProUGUI>();
        DashControl = Resources.Load<Control>("Managers/InputManager/Dash");
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            UnityEngine.Time.timeScale = 0.01f;
            _tutorialText.enabled = true;
            if (DashControl.IsMouseButton)
            {
                _tutorialText.text = "Press " + DashControl.MouseButton + " to dash!";
            }
            else
            {
                _tutorialText.text = "Press " + DashControl.KeyCode + " to dash!";
            }
            StartCoroutine(WaitForDash());
        }
    }

    IEnumerator WaitForDash()
    {
        Debug.Log("TEST");
        while (!DashControl.JustPressed)
        {
            yield return new WaitForEndOfFrame();
        }
        UnityEngine.Time.timeScale = 1;
        _tutorialText.enabled = false;
        Destroy(gameObject);
    }
}
