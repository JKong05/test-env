using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MicManager : MonoBehaviour
{
    private string selectedMicrophone;
    public TextMeshProUGUI microphoneStatusText; // Reference to a UI Text element for displaying status

    void Start()
    {
        // Get and log the list of available microphones
        string[] microphoneDevices = Microphone.devices;
        if (microphoneDevices.Length > 0)
        {
            // Automatically select the first microphone
            selectedMicrophone = microphoneDevices[1]; 
            microphoneStatusText.text = $"Selected Microphone: {selectedMicrophone}";
            Debug.Log($"Selected Microphone: {selectedMicrophone}");
        }
        else
        {
            microphoneStatusText.text = "No microphones found.";
            Debug.Log("No microphones found.");
        }
    }
}
