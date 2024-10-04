using UnityEngine;
using UnityEngine.XR;

public class HandTrackingVisualizer : MonoBehaviour
{
    public GameObject leftHandMarker;
    public GameObject rightHandMarker;

    private void Update()
    {
        // Update the positions of the hand markers
        UpdateHandMarker(XRNode.LeftHand, leftHandMarker);
        UpdateHandMarker(XRNode.RightHand, rightHandMarker);
    }

    private void UpdateHandMarker(XRNode hand, GameObject marker)
    {
        InputDevice device = InputDevices.GetDeviceAtXRNode(hand);
        if (device.isValid)
        {
            // Try to get the position and rotation of the hand
            if (device.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position) &&
                device.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation))
            {
                // Set the marker position to the hand position
                marker.transform.position = position;

                // If you want the marker to align with the hand's rotation
                marker.transform.rotation = rotation;

                // Optionally, if you're working in a local space, you might need to adjust:
                // Vector3 worldPosition = Camera.main.transform.TransformPoint(position);
                // marker.transform.position = worldPosition;
            }
        }
    }
}
