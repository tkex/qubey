using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            // Deactivate both game objects
            gameObject.SetActive(false);

            other.gameObject.SetActive(false);
        }
    }
}
