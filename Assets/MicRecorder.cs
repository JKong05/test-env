using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class MicRecorder : MonoBehaviour
{
    private AudioClip _audioClip;
    private string _micDevice;
    private bool _isRecording = false;

    // Reference to the UI button and text
    // public Button startStopButton;
    // public TextMeshProUGUI buttonText;

    void Start()
    {
        // Set up the button listener
        // startStopButton.onClick.AddListener(OnButtonClick);

        string path = Application.persistentDataPath;
        // Check if the directory exists
        if (!Directory.Exists(path))
        {
            // Create the directory if it doesn't exist
            Directory.CreateDirectory(path);
            Debug.Log("Directory created: " + path);
        }
        else
        {
            Debug.Log("Directory already exists: " + path);
        }
    }

    // Start recording
    public void StartRecording()
    {
        if (Microphone.devices.Length > 0)
        {
            _micDevice = Microphone.devices[0];
            _audioClip = Microphone.Start(_micDevice, false, 300, 44100); // Recording buffer for up to 5 minutes
            _isRecording = true;
            Debug.Log("Recording started.");
        }
        else
        {
            Debug.LogError("No microphone devices found.");
        }
    }

    // Stop recording and save
    public void StopRecording(int participantNum, int storynum, int storyType)
    {
        if (_isRecording)
        {
            int recordedSamples = Microphone.GetPosition(_micDevice); // Get how many samples were recorded
            Microphone.End(_micDevice);

            // Trim the clip to the actual recorded length
            AudioClip trimmedClip = TrimAudioClip(_audioClip, recordedSamples);
            string recordingsPath = Path.Combine(Application.persistentDataPath, "RecordingsFolder");
            string participantFolder = Path.Combine(recordingsPath, "p" + participantNum.ToString());

            // Ensure the folder exists
            if (!Directory.Exists(participantFolder))
            {
                Directory.CreateDirectory(participantFolder);
            }
            //storytype : 0 - audio, 1 - visual, 2 = audiovisual
            String modality = storyType == 0 ? "audio" : storyType == 1 ? "video" : storyType == 2 ? "audiovisual" : "Unknown Type";
            SaveWavToSpecificFolder(trimmedClip, $"p{participantNum}_story{storynum}_{modality}.wav", participantFolder);
            _isRecording = false;

            Debug.Log("Recording stopped and saved.");
        }
    }

    // Trim the AudioClip to the recorded length
    private AudioClip TrimAudioClip(AudioClip clip, int recordedSamples)
    {
        float[] samples = new float[recordedSamples];
        clip.GetData(samples, 0);

        AudioClip trimmedClip = AudioClip.Create(clip.name, recordedSamples, clip.channels, clip.frequency, false);
        trimmedClip.SetData(samples, 0);

        return trimmedClip;
    }

    // Save the AudioClip to a specific folder
    public static void SaveWavToSpecificFolder(AudioClip clip, string fileName, string folderName)
    {
        if (clip == null)
        {
            Debug.LogError("AudioClip is null, cannot save.");
            return;
        }

        // Get the path to the persistent data directory
        string folderPath = Path.Combine(Application.persistentDataPath, folderName);

        // Ensure the folder exists, create it if it doesn't
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Generate a unique file name
        string uniqueFileName = GetUniqueFileName(folderPath, fileName);

        // Define the full file path
        string filePath = Path.Combine(folderPath, uniqueFileName);

        // Convert the AudioClip to wav bytes and save
        var samples = new float[clip.samples];
        clip.GetData(samples, 0);

        var wavBytes = ConvertAudioClipToWav(samples, clip.channels, clip.frequency);
        File.WriteAllBytes(filePath, wavBytes);

        Debug.Log("Recording saved at: " + filePath);
    }

    private static string GetUniqueFileName(string folderPath, string fileName)
    {
        // Remove the extension from the file name
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        string extension = Path.GetExtension(fileName);

        // Start with the base file name
        string uniqueFileName = fileName;
        int fileCount = 1;

        // Check if a file with the same name already exists
        while (File.Exists(Path.Combine(folderPath, uniqueFileName)))
        {
            // If the current name has a number, increment it
            if (uniqueFileName.Contains("("))
            {
                int startIndex = uniqueFileName.LastIndexOf('(');
                int endIndex = uniqueFileName.LastIndexOf(')');
                if (endIndex > startIndex)
                {
                    string numberPart = uniqueFileName.Substring(startIndex + 1, endIndex - startIndex - 1);
                    if (int.TryParse(numberPart, out int number))
                    {
                        fileCount = number + 1; // Increment the number
                        uniqueFileName = uniqueFileName.Substring(0, startIndex) + $"({fileCount})" + extension;
                        continue; // Continue the loop to check the new name
                    }
                }
            }

            // If there isn't a number in the name, append one
            uniqueFileName = $"{fileNameWithoutExtension}({fileCount}){extension}";
            fileCount++;
        }

        return uniqueFileName;
    }

    private static byte[] ConvertAudioClipToWav(float[] samples, int channels, int frequency)
    {
        using (var memoryStream = new MemoryStream())
        {
            int sampleCount = samples.Length;
            int byteRate = frequency * channels * 2; // bytes per second

            // Write WAV header (as before)
            memoryStream.Write(new byte[44], 0, 44);

            // Convert float samples to PCM 16-bit format
            foreach (var sample in samples)
            {
                short intSample = (short)(sample * short.MaxValue);
                memoryStream.WriteByte((byte)(intSample & 0xff));
                memoryStream.WriteByte((byte)((intSample >> 8) & 0xff));
            }

            var wavBytes = memoryStream.ToArray();
            WriteWavHeader(wavBytes, sampleCount, channels, frequency, byteRate);

            return wavBytes;
        }
    }

    private static void WriteWavHeader(byte[] bytes, int sampleCount, int channels, int frequency, int byteRate)
    {
        using (var memoryStream = new MemoryStream(bytes))
        {
            BinaryWriter writer = new BinaryWriter(memoryStream);
            writer.Seek(0, SeekOrigin.Begin);

            writer.Write(new[] { 'R', 'I', 'F', 'F' });
            writer.Write(36 + sampleCount * channels * 2);
            writer.Write(new[] { 'W', 'A', 'V', 'E' });

            writer.Write(new[] { 'f', 'm', 't', ' ' });
            writer.Write(16); // Subchunk1Size (PCM = 16)
            writer.Write((short)1); // AudioFormat (PCM = 1)
            writer.Write((short)channels);
            writer.Write(frequency);
            writer.Write(byteRate);
            writer.Write((short)(channels * 2)); // BlockAlign (channels * bytesPerSample)
            writer.Write((short)16); // BitsPerSample

            writer.Write(new[] { 'd', 'a', 't', 'a' });
            writer.Write(sampleCount * channels * 2);
        }
    }
}
