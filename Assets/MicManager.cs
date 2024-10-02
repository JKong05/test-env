using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MicManager : MonoBehaviour
{
    private string selectedMicrophone;
    public TextMeshProUGUI microphoneStatusText; // Reference to a UI Text element for displaying status
<<<<<<< HEAD
    private AudioClip audioClip;
=======
>>>>>>> tevin

    void Start()
    {
        // Get and log the list of available microphones
        string[] microphoneDevices = Microphone.devices;
        if (microphoneDevices.Length > 0)
        {
            // Automatically select the first microphone
            selectedMicrophone = microphoneDevices[0]; 
            microphoneStatusText.text = $"Selected Microphone: {selectedMicrophone}";
            Debug.Log($"Selected Microphone: {selectedMicrophone}");
        }
        else
        {
            microphoneStatusText.text = "No microphones found.";
            Debug.Log("No microphones found.");
        }
    }
<<<<<<< HEAD

    public void StartRecording()
    {
        if (!string.IsNullOrEmpty(selectedMicrophone))
        {
            audioClip = Microphone.Start(selectedMicrophone, true, 10, 44100);
            microphoneStatusText.text = $"Recording on: {selectedMicrophone}";
            Debug.Log($"Recording started on {selectedMicrophone}");
        }
        else
        {
            microphoneStatusText.text = "No microphone selected for recording.";
            Debug.Log("No microphone selected for recording.");
        }
    }

    public void StopRecording()
    {
        if (!string.IsNullOrEmpty(selectedMicrophone))
        {
            Microphone.End(selectedMicrophone);
            microphoneStatusText.text = $"Stopped recording on: {selectedMicrophone}";
            Debug.Log($"Recording stopped on {selectedMicrophone}");
        }
    }
=======
>>>>>>> tevin
}
