using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleTowardCenter : MonoBehaviour
{
    public Transform targetObject;  // The external object used as the center reference
    public float maxScale = 2f;     // Maximum scale when at the center
    public float minScale = 0.5f;   // Minimum scale when farthest
    public float scaleSpeed = 5f;   // Speed of scaling effect
    public float maxDistance = 500f; // Custom max distance for scaling effect
    public float xOffset = 0f;      // X-axis offset for center reference

    private RectTransform parentRect;

    void Start()
    {
        parentRect = GetComponent<RectTransform>();

        if (parentRect == null)
        {
            Debug.LogError("This script must be attached to a UI Parent with a RectTransform!");
            return;
        }

        if (targetObject == null)
        {
            Debug.LogError("Target Object is not assigned! Set an object for scaling reference.");
            return;
        }
    }

    void Update()
    {
        if (targetObject == null) return;

        // Loop through each child and adjust scale
        foreach (RectTransform child in parentRect)
        {
            ScaleChild(child);
        }
    }

    void ScaleChild(RectTransform child)
    {
        if (child == null) return;

        // Adjust the target position with X-axis offset
        Vector3 targetPosition = targetObject.position + new Vector3(xOffset, 0, 0);

        // Get the current distance from the adjusted target position
        float currentDistance = Vector2.Distance(child.position, targetPosition);

        // Calculate a normalized distance factor (0 at center, 1 at maxDistance)
        float normalizedDistance = Mathf.Clamp01(currentDistance / maxDistance);

        // Apply symmetry: If the child passes the center, make it shrink back
        float scaleFactor = Mathf.Lerp(maxScale, minScale, Mathf.Abs(1 - normalizedDistance * 2));

        // Smoothly interpolate scale change
        child.localScale = Vector3.Lerp(child.localScale, Vector3.one * scaleFactor, Time.deltaTime * scaleSpeed);
    }
}
