using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class delLaterTest : MonoBehaviour
{
    public GameObject cubePrefab; // Reference to the cube prefab

    void Update()
    {
        // Check for key press "S"
        if (Input.GetKeyDown(KeyCode.S))
        {
            SpawnCube(Vector3.zero); // Spawn a cube at the origin (0, 0, 0)
        }
    }

    // Function to spawn a cube at a given position
    public void SpawnCube(Vector3 position)
    {
        if (cubePrefab != null)
        {
            // Instantiate a cube at the specified position and rotation
            Instantiate(cubePrefab, position, Quaternion.identity);
            Debug.Log("Cube spawned at: " + position);
        }
        else
        {
            Debug.LogError("Cube prefab is not assigned.");
        }
    }
}
