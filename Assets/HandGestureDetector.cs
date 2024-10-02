// using UnityEngine;
// using UnityEngine.XR;

// public class HandGestureDetector : MonoBehaviour
// {
//     private void Update()
//     {
//         // Assuming you have access to hand tracking data
//         if (TryGetHandData(out Vector3 thumbPosition, out Vector3 palmPosition))
//         {
//             if (IsThumbsUp(thumbPosition, palmPosition))
//             {
//                 Debug.Log("Thumbs Up detected!");
//             }
//         }
//     }

//     private bool TryGetHandData(out Vector3 thumbPosition, out Vector3 palmPosition)
//     {
//         // Implement logic to retrieve hand positions from XR input
//         // This is a placeholder. Replace with actual hand tracking API.
//         thumbPosition = Vector3.zero; // Replace with actual thumb position
//         palmPosition = Vector3.zero; // Replace with actual palm position
//         return true; // Return true if hand data is successfully retrieved
//     }

//     private bool IsThumbsUp(Vector3 thumbPosition, Vector3 palmPosition)
//     {
//         // Implement your logic for detecting a thumbs-up gesture
//         // For example, check if the thumb is above the palm position
//         return thumbPosition.y > palmPosition.y;
//     }
// }

using UnityEngine;
using UnityEngine.XR;

public class HandTracking : MonoBehaviour
{
    private void Update()
    {
        // Get the left and right hand positions
        Vector3 leftHandPosition = GetHandPosition(XRNode.LeftHand);
        Vector3 rightHandPosition = GetHandPosition(XRNode.RightHand);

        // Log hand positions to the console
        Debug.Log("Left Hand Position: " + leftHandPosition);
        Debug.Log("Right Hand Position: " + rightHandPosition);
    }

    private Vector3 GetHandPosition(XRNode hand)
    {
        // Get the input device for the specified hand
        InputDevice device = InputDevices.GetDeviceAtXRNode(hand);
        if (device.isValid)
        {
            // Try to get the position of the hand
            if (device.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position))
            {
                return position;
            }
        }
        return Vector3.zero; // Return zero if hand tracking is not valid
    }
}
