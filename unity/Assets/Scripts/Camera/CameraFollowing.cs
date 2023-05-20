using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    // The Transform component of the target object to follow
    public Transform target; 

    // The smoothness of the cameras movement
    public float smoothSpeed = 0.125f;

    // The offset of the camera from the target object
    public Vector3 offset; 

    // FixedUpdate is called every fixed framerate frame
    void FixedUpdate()
    {
        // Calculate the position of the camera based on the target position and the offset
        Vector3 desiredPosition = target.position + offset;

        // Smoothly move the camera to the desired position using Lerp
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Make the camera look at the target object
        transform.LookAt(target);
    }
}