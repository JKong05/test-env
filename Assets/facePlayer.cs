using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class facePlayer : MonoBehaviour
{
    public GameObject playerCamera;
    public float radius;        // Distance from the camera (radius of the circular path)
    public float smoothTime;    // Time for smooth interpolation
    public float angleSpeed;  // Speed at which to move in the arc

    private float currentAngle;     // Current angle around the camera
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Get the direction the camera is facing
        Vector3 cameraForward = Camera.main.transform.forward;

        // Flatten the camera forward vector to the horizontal plane (ignore Y component)
        cameraForward.y = 0.0f;
        cameraForward.Normalize();

        // Calculate the target angle in degrees (where the camera is looking)
        float targetAngle = Mathf.Atan2(cameraForward.z, cameraForward.x) * Mathf.Rad2Deg;

        // Smoothly move the current angle towards the target angle using Lerp
        currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, angleSpeed * Time.deltaTime);

        // Calculate the target position based on the current angle
        Vector3 targetPosition = Camera.main.transform.position + new Vector3(
            Mathf.Cos(currentAngle * Mathf.Deg2Rad) * radius,
            0,
            Mathf.Sin(currentAngle * Mathf.Deg2Rad) * radius
        );

        // Smoothly move the object towards the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothTime * Time.deltaTime);

        // Optional: Face the camera
        transform.LookAt(Camera.main.transform);
    }
}
