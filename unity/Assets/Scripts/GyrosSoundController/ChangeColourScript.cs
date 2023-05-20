using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GyroSoundController))]
public class ChangeColourScript : MonoBehaviour
{
    // Colour that can be assigned in the Inspector
    public Color color;

    // How long the colour will be applied (definable in the Inspector)
    public float colourDuration;

    // Original colour of the GameObject
    private Color originalColor;

    // Flag to check whether the colour is currently being changed
    private bool changingColor = false;

    // Remaining time for the colour to be applied
    private float remainingDuration; 

    void Start()
    {
        // Save the original colour of the GameObject
        originalColor = GetComponent<Renderer>().material.color;
    }

    void Update()
    {
        // If the "changingColor" flag is active...
        if (changingColor)
        {
            // Decrement the remaining duration by the elapsed time
            remainingDuration -= Time.deltaTime;

            // If the remaining duration has elapsed...
            if (remainingDuration <= 0)
            {
                // ... restore the original color
                GetComponent<Renderer>().material.color = originalColor;

                // Deactivate flag
                changingColor = false;
            }
        }
    }

    public void ChangeColor()
    {
        // Set the color to the one assigned in the Inspector
        GetComponent<Renderer>().material.color = color;

        // Activate flag
        changingColor = true;

        // Set the remaining duration
        remainingDuration = colourDuration;
    }
}
