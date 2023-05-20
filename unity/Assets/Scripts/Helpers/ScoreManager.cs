
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    // Reference to the text objects that display the score and winning text
    public TextMeshProUGUI winningText;
    public TextMeshProUGUI pointsDisplay;

    // The current (initial) score of the player
    public int totalPoints = 0;

    // The scene that should be loaded when the timer is finished
    public string nextScene;

    // The amount of time in seconds to wait before loading the next scene
    public float loadSceneDelay;

    // Hides the coin
    private void HideCoin(GameObject coin)
    {
        coin.SetActive(false);
    }

    // This method is called when the game object collides with another collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the other game object has the "WinningArea" tag
        if (other.gameObject.tag == "WinningArea")
        {
            // Call the WinningAreaHasBeenHit coroutine
            StartCoroutine(WinningAreaHasBeenHit());
        }

        if (other.gameObject.tag == "Coin")
        {
            HideCoin(other.gameObject);
            AddPoints(10);
        }
    }

    // Adds the points and updates the display
    private void AddPoints(int pointsToAdd)
    {
        totalPoints += pointsToAdd;
        pointsDisplay.text = "Points: " + totalPoints.ToString();
    }

    // This method is called when the game object collides with another game object
    private void OnCollisionEnter(Collision other)
    {
        // If there is a collision with an object with tag "Point_1", add 10 points
        if (other.gameObject.CompareTag("Point_1"))
        {
            HideCoin(other.gameObject);
            AddPoints(10);
        }

        // If there is a collision with an object with tag "Point_2", add 20 points
        if (other.gameObject.CompareTag("Point_2"))
        {
            HideCoin(other.gameObject);
            AddPoints(20);
        }

        // If there is a collision with an object with tag "Point_3", add 50 points
        if (other.gameObject.CompareTag("Point_3"))
        {
            HideCoin(other.gameObject);
            AddPoints(50);
        }

        // Check if the other game object has "Block" tag
        if (other.gameObject.tag == "Block")
        {
            // Call the BlockOrDeadHasBeenHit coroutine
            StartCoroutine(BlockOrDeadHasBeenHit());
        }
    }


    void OnTriggerStay(Collider other)
    {
        // Check if the other collider has an "Enemy" tag
        if (other.gameObject.tag == "DropStation")
        {
            // If honked song is placed
            HonkScript honkScriptObject = GetComponent<HonkScript>();
            bool canHonk = honkScriptObject.canHonk;

            // If honk cannot be played, increase the score, update the score text and deactivate the drop station
            if (!canHonk)
            {
                totalPoints += 1;
                pointsDisplay.text = "Dropstation: " + totalPoints + "/5";
                other.gameObject.SetActive(false);

                // If score reaches 5, start the WinningAreaHasBeenHit coroutine
                if (totalPoints == 5)
                {
                    StartCoroutine(WinningAreaHasBeenHit());
                }
            }
        }
    }

    IEnumerator WinningAreaHasBeenHit()
    {
        // Display winning text for five seconds
        for (int i = 5; i > 0; i--)
        {
            winningText.gameObject.SetActive(true);
            winningText.text = "You won! :)";

 
            // Deactivate game timer
            GameObject.Find("GameTimer").GetComponent<GameTimer>().enabled = false;

            // Deactivate the gyro sound controller script
            GameObject.Find("Qubey").GetComponent<GyroSoundController>().enabled = false;

            yield return new WaitForSeconds(1.0f);
        }

        // Load the next scene
        // Invoke("LoadNextScene", loadSceneDelay);

        // Reload the current scene
        // UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    IEnumerator BlockOrDeadHasBeenHit()
    {
        Debug.Log("Block Collision!");

        // Display losing text
        winningText.gameObject.SetActive(true);
        winningText.text = "You lose!";

        // Deactivate game timer
        GameObject.Find("GameTimer").GetComponent<GameTimer>().enabled = false;

        // Deactivate the gyro sound controller script
        GameObject.Find("Qubey").GetComponent<GyroSoundController>().enabled = false;

        // Wait for 3 seconds
        for (int i = 1; i > 0; i--)
        {
            yield return new WaitForSeconds(3.0f);
        }

        // Load the next scene
        // UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);

        // Reload the current scene
        // Invoke("LoadNextScene", loadSceneDelay);
    }

    // Loads the next scene
    void LoadNextScene()
    {
        // Load the next scene
        SceneManager.LoadScene(nextScene);
    }
}