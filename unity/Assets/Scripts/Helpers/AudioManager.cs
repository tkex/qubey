
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Drag and drop your background music audio clip in the Inspector
    public AudioClip backgroundMusic;

    // Set the volume of the background music in the Inspector
    [Range(0, 1)]
    public float backgroundMusicVolume = 1;

    // Drag and drop your sound effect audio clips in the Inspector
    //public AudioClip endGameSoundEffect;
    public AudioClip honkSoundEffect;
    public AudioClip jumpSoundEffect;
    public AudioClip shootSoundEffect;
    public AudioClip coinSoundEffect;

    // Set the volume of the sound effects in the Inspector
    [Range(0, 1)]
    public float soundEffectVolume = 1;

    // Reference to the audio source component
    private AudioSource audioSource;

    void Start()
    {
        // Get the audio source component
        audioSource = GetComponent<AudioSource>();

        if (backgroundMusic != null)
        {
            // Play the background music
            audioSource.clip = backgroundMusic;
            audioSource.volume = backgroundMusicVolume;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    /*
    // Play the sound effect
    public void PlaySoundEffect()
    {
        audioSource.clip = endGameSoundEffect;
        audioSource.volume = soundEffectVolume;
        audioSource.Play();
    }
    */

    // Play the honk sound effect
    public void PlayHonkSound()
    {
        // Set the honk sound effect and play it once
        audioSource.PlayOneShot(honkSoundEffect);
    }

    // Play the jump sound effect
    public void PlayJumpSound()
    {
        // Set the jump sound effect and play it once
        audioSource.PlayOneShot(jumpSoundEffect);
    }

    // Play the shoot sound effect
    public void PlayShootSound()
    {
        // Set the shoot sound effect and play it once
        audioSource.PlayOneShot(shootSoundEffect);
    }

    // Play the coin collection sound effect
    public void PlayCoinCollectSound()
    {
        // Set the coin collection sound effect and play it once
        audioSource.PlayOneShot(coinSoundEffect);
    }
}