using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Deactivate both game objects
            gameObject.SetActive(false);

            collision.gameObject.SetActive(false);
        }
    }
}