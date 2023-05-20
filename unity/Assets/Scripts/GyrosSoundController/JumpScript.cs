using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GyroSoundController))]
public class JumpScript : MonoBehaviour
{
    // The height the character should jump
    public float jumpHeight = 5f;

    // Zime interval in which the player cannot jump after jumping
    public float jumpCooldown = 2f;

    // Timer to keep track of the jump cooldown
    private float jumpTimer;

    // Flag to keep track of whether the player is allowed to jump
    private bool canJump = true;

    // Reference to AudioManager script
    public AudioManager audioManager;

    void Start()
    {
        // By default, the player can jump at the start of the game
        // canJump = true;
    }

    // FixedUpdate is called at fixed intervals
    void FixedUpdate()
    {
        // If the player is not allowed to jump, decrease the jump timer
        if (!canJump)
        {
            jumpTimer -= Time.deltaTime;

            // If the jump timer reaches zero, the player can jump again
            if (jumpTimer <= 0)
            {
                canJump = true;
            }
        }
    }

    // Jump function that can be called from other scripts
    public void Jump()
    {
        // If the player is allowed to jump, make the player jump
        if (canJump)
        {
            //Play jump sound effect
            if (audioManager.jumpSoundEffect != null)
            {
                audioManager.PlayJumpSound();
            }

            // Add upward force to the players rigidbody
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);

            // Set the jump timer and disable jumping
            jumpTimer = jumpCooldown;

            canJump = false;
        }
    }
}