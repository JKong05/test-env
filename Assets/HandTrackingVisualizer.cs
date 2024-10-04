using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HandTrackingVisualizer : MonoBehaviour
{
    public GameObject leftHandMarker;  // Reference to the left hand marker GameObject
    public GameObject rightHandMarker; // Reference to the right hand marker GameObject

    private InputDevice leftHandDevice;
    private InputDevice rightHandDevice;

    private void Start()
    {
        // Get the left and right hand devices
        leftHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        rightHandDevice = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
    }

    private void Update()
    {
        // Update the hand positions
        UpdateHandMarker(XRNode.LeftHand, leftHandMarker);
        UpdateHandMarker(XRNode.RightHand, rightHandMarker);

        // Detect gestures
        DetectGestures(leftHandDevice);
        DetectGestures(rightHandDevice);
    }

    private void UpdateHandMarker(XRNode hand, GameObject marker)
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(hand);
        if (device.isValid)
        {
            if (device.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position))
            {
                marker.transform.position = position;
            }
        }
    }

    private void DetectGestures(InputDevice handDevice)
    {
        if (handDevice.isValid)
        {
            // Example: Pinch Gesture
            if (IsPinching(handDevice))
            {
                Debug.Log("Pinch Gesture Detected");
            }

            // Example: Open Hand Gesture
            if (IsHandOpen(handDevice))
            {
                Debug.Log("Open Hand Gesture Detected");
            }
        }
    }

    private bool IsPinching(InputDevice handDevice)
    {
        // Check if the grip button is pressed (you can also check thumb/index finger positions if available)
        return handDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool isGripping) && isGripping;
    }

    private bool IsHandOpen(InputDevice handDevice)
    {
        // Check if the grip button is not pressed
        return handDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool isGripping) && !isGripping;
    }
}
