using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwayingScript : MonoBehaviour
{
    // Set the speed of the movement in the Inspector
    public float movementSpeed = 1.0f;

    void Update()
    {
        // Get the current position of the GameObject
        Vector3 currentPosition = transform.position;

        // Calculate the new position using sin function
        float newY = Mathf.Sin(Time.time * movementSpeed);

        // Set the new position using the calculated value for the y-axis
        transform.position = new Vector3(currentPosition.x, newY, currentPosition.z);
    }
}
