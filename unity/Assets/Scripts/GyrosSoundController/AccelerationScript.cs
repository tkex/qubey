using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Requires a GyroSoundController component to be attached to this object
[RequireComponent(typeof(GyroSoundController))]
public class AccelerationScript : MonoBehaviour
{

    // Speed to set the speed to
    [Range(1.0f, 500.0f)]
    [Tooltip("The forward speed to set.")]
    public float accelerationAmount = 500.0f;

    // Time to wait before resetting the speed
    [Range(1.0f, 10.0f)]
    [Tooltip("The time in seconds to wait before resetting the speed.")]
    public float resetTime = 8.0f;

    // Initial speed to reset to
    private float initialSpeed;

    // A timer to keep track of the speed cooldown
    private float jumpTimer;

    // Flag to keep track of whether the player is allowed to speed up
    private bool canJump = false;

    // Flag to keep track of whether the timer has ended
    private bool timerIsDone = false;

    private void Start()
    {
        // Wait for 5 seconds before setting canJump to true
        StartCoroutine(WaitAndLoadCoroutine());
    }

    void FixedUpdate()
    {
        // If the player can jump and the timer has ended
        if (canJump && timerIsDone)
        {
            // Move the game object forward
            transform.position += transform.forward * accelerationAmount * Time.deltaTime;

            // Set canJump to false to prevent multiple jumps in a row
            canJump = false;
        }
        else if (!canJump && timerIsDone)
        {
            // If the player can't jump but the timer has ended, start the cooldown
            jumpTimer -= Time.deltaTime;

            // If the cooldown has ended, allow the player to jump again
            if (jumpTimer <= 0)
            {
                canJump = true;
            }
        }
    }

    public void Accelerate()
    {
        // If the player is allowed to jump, make the character jump
        if (canJump)
        {
            // Set the jump timer and disable jumping until the timer ends
            jumpTimer = resetTime;

            canJump = false;

            // Set the timerIsDone flag to true to prevent jumping until the timer resets
            timerIsDone = true;
        }
    }

    // Coroutine that waits for 5 seconds before setting the timerIsDone flag to true
    IEnumerator WaitAndLoadCoroutine()
    {
        for (int i = 5; i > 0; i--)
        {
            // Wait for 1 second
            yield return new WaitForSeconds(1.0f);
        }

        // Set the timerIsDone flag to true after the 5 second delay
        timerIsDone = true;

        canJump = false;
    }
}