using System.Collections;
using UnityEngine;
using TMPro;

public class WaitAndLoad : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    private bool timerIsCounted = false;


    void Start()
    {
        StartCoroutine(WaitAndLoadCoroutine());
    }

    IEnumerator WaitAndLoadCoroutine()
    {
        // Loop through a countdown from 5 seconds
        for (int i = 5; i > 0; i--)
        {
            // Timer info
            // Update the timer text to show the countdown
            timerText.text = "Ready in..." + i;

            // Wait for 1 second
            yield return new WaitForSeconds(1.0f);

            // Set the timerIsCounted variable to true
            timerIsCounted = true;
        }

        // If the timer has been counte
        if (timerIsCounted)
        {
            // Timer info
            // Update the timer text
            timerText.text = "GO!";

            // Wait for 1 second
            yield return new WaitForSeconds(1.0f);

        }

        // Disable the timer text
        timerText.enabled = false; 

        // Activate countdown script
        GameObject.Find("GameTimer").GetComponent<GameTimer>().enabled = true;

        // Uncomment line to activate the GyroSoundController script
        // GameObject.Find("Qubey").GetComponent<GyroSoundController>().enabled = true;
    }
}