using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// PlayerScript requires the GameObject to have a Rigidbody component
[RequireComponent(typeof(GyroSoundController))]
public class ShootScript : MonoBehaviour
{
    // Declare the bullet prefab as a serialized field that can be assigned in the Inspector
    [SerializeField]
    private GameObject bulletPrefab;

    // Declare the prefab where the shoot is coming from
    [SerializeField]
    private GameObject gunPrefab;

    // The speed of the bullet
    public float bulletSpeed = 5f;

    // Time interval in which the player cannot shoot after shooting
    public float shootCooldown = 2f;

    // Timer to keep track of the shoot cooldown
    private float shootTimer;

    // Flag to keep track of whether the player is allowed to shoot
    private bool canShoot = true;

    // Reference to Audio
    public AudioManager audioManager;

    void FixedUpdate()
    {
        // If the player is not allowed to shoot, decrease the shoot timer
        if (!canShoot)
        {
            shootTimer -= Time.deltaTime;

            // If the shoot timer reaches zero, the player can shoot again
            if (shootTimer <= 0)
            {
                canShoot = true;
            }
        }
    }

    // The method to shoot the bullet that can be called from other scripts
    public void Jump()
    {
        // If the player is allowed to shoot, make the character able to shoot
        if (canShoot)
        {
            Debug.Log("Shoot!");

            if(audioManager.shootSoundEffect != null)
            {
                // Play the shoot sound
                audioManager.PlayShootSound();
            }

            // Instantiate the bullet prefab at the position of the transform of the gun prefab
            GameObject bullet = Instantiate(bulletPrefab, gunPrefab.transform.position, Quaternion.identity);

            // Get the Rigidbody component of the bullet prefab and apply the bullet speed to it
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = transform.TransformDirection(Vector3.forward) * bulletSpeed;


            // Set the shoot timer and disable shooting
            shootTimer = shootCooldown;

            canShoot = false;
        }
    }
}