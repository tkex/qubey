using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GyroSoundController))]
public class HonkScript : MonoBehaviour
{
    // The volume of the sound
    [Range(0.0f, 1.0f)]
    public float volume = 1f;

    // Time to wait before the sound can be played again (in seconds)
    public float honkCooldown = 2f;

    // Timer to keep track of the honk cooldown
    private float honkTimer;

    // Flag to keep track whether the player is allowed to honk 
    public bool canHonk = true;

    // Reference to AudioManager
    public AudioManager audioManager;

    // FixedUpdate is called every fixed framerate frame
    private void FixedUpdate()
    {
        // If the player is not allowed to honk, decrease the honk timer
        if (!canHonk)
        {
            honkTimer -= Time.deltaTime;

            // If the honk timer reaches zero, the player can honk again
            if (honkTimer <= 0)
            {
                canHonk = true;
            }
        }
    }

    // Honk function called when player wants to honk
    public void Honk()
    {
        // If the player is allowed to honk, play the honk sound
        if (canHonk)
        {
            // Play honk sound using AudioManager
            if(audioManager.honkSoundEffect != null)
            {
                audioManager.PlayHonkSound();
            }

            // Set the honk timer and disable honking
            honkTimer = honkCooldown;

            canHonk = false;
        }
    }
}