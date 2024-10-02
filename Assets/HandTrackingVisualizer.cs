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
            if (device.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position))
            {
                marker.transform.position = position;
            }
        }
    }
}
