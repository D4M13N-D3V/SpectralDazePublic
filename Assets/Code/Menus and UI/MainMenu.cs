using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace SpectralDaze.Menu
{
    /// <summary>
    /// The main menu of the game.
    /// </summary>
    /// <seealso cref="UnityEngine.MonoBehaviour" />
    public class MainMenu : MonoBehaviour
    {
        public string SceneName;
        float timer = 0f;
        float timerMax = .05f;
        private bool pressedPlay = false;

        public GameObject BlackScreen;


        // Update is called once per frame
        void Update()
        {
            if (!pressedPlay)
                return;
            timer += UnityEngine.Time.deltaTime;

            if (timer >= timerMax)
            {
                var color = BlackScreen.GetComponent<Image>().color;
                BlackScreen.GetComponent<Image>().color = new Color(color.r, color.g, color.b, color.a + .04f);
                timer = 0;
                Debug.Log(color.a);
            }

            if (BlackScreen.GetComponent<Image>().color.a >= 1f)
                SceneManager.LoadScene(SceneName);
        }

        public void Play()
        {
            BlackScreen.SetActive(true);
            pressedPlay = true;
        }
    }
}
