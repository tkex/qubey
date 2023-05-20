using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameTimer : MonoBehaviour
{
    // The time in seconds that the timer should run for
    public float timeInSeconds;

    // The UI text element that will display the time
    public TextMeshProUGUI timerText;

    // The UI text element that will display the "Time is over!" message
    public TextMeshProUGUI gameOverText;

    // The scene that should be loaded when the timer is finished
    public string nextScene;

    // The amount of time in seconds to wait before loading the next scene
    public float loadSceneDelay;

    // Flag to track whether the timer is currently running
    private bool timerIsRunning;

    void Start()
    {
        // Set the timerIsRunning flag to true to indicate that the timer is running
        timerIsRunning = true;

        // Set the gameOverText to be inactive
        gameOverText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (timerIsRunning)
        {

            if (timeInSeconds > 0)
            {
                // Decrement the time remaining
                timeInSeconds -= Time.deltaTime;

                // Update the timer text
                timerText.text = "Timer: " + Mathf.Floor(timeInSeconds) + " Sec.";
            }

            // If the time remaining has reached 0
            if (timeInSeconds <= 0)
            {
                timerText.text = "Timer: 0 Sec.";

                // Set the timerIsRunning flag to false to stop the timer
                timerIsRunning = false;

                // Set the gameOverText to be active
                gameOverText.gameObject.SetActive(true);
                gameOverText.text = "Time is out!";

                // Deactivate the gyro sound controller script
                GameObject.Find("Qubey").GetComponent<GyroSoundController>().enabled = false;

                // Invoke the LoadNextScene method after the specified delay
                //Invoke("LoadNextScene", loadSceneDelay);

            }

        }
    }
}