using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GyroSoundController))]
public class MoveForward : MonoBehaviour
{
    // The speed that the object will move at
    [Range(0, 250)]
    public float speed = 1.0f;

    // Update is called once per frame
    void Update()
    {
        // Move the object forward based on speed and time
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}