using UnityEngine;

public class DirectCameraFollowing : MonoBehaviour
{
    // The offset of the camera from the target
    private Vector3 _offset;

    // The target to follow
    [SerializeField] private Transform target;

    // The time it takes for the camera to smoothly move to the target
    [SerializeField] private float smoothTime;

    // The current velocity of the camera
    private Vector3 _currentVelocity = Vector3.zero;

    private void Awake()
    {
        // Calculate the offset
        _offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        // Calculate the target position based on the offset
        Vector3 targetPosition = target.position + _offset;

        // Smoothly move the camera towards the target position using SmoothDamp
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
    }
}