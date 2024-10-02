using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayAudio2 : MonoBehaviour
{
    private bool _isAudioPlaying = false;

    // Reference to the UI button, text and audio
    public Button audioButton;
    public TextMeshProUGUI buttonText;
    public AudioSource soundPlayer;

    // Start is called before the first frame update
    void Start()
    {
        // Set up the button listener
        audioButton.onClick.AddListener(OnButtonClick);   
    }

    // Handle button click
    public void OnButtonClick()
    {
        if (_isAudioPlaying)
        {
            _isAudioPlaying = false;
            pauseThisAudio();
            buttonText.text = "Audio Paused";
        }
        else
        {
            _isAudioPlaying = true;
            playThisAudio();
            buttonText.text = "Audio Playing";
        }
    }

    public void playThisAudio()
    {
        soundPlayer.Play();
    }

    public void pauseThisAudio()
    {
        soundPlayer.Pause();
    }
}
