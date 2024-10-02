using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
<<<<<<< HEAD
using System.IO;
=======
>>>>>>> tevin

public class PlayAudio : MonoBehaviour
{
    private bool _isAudioPlaying = false;

<<<<<<< HEAD
    
    public Button audioButton;
    public TextMeshProUGUI buttonText;
    private bool _isAudioPlaying = false;

=======
    // Reference to the UI button, text and audio
    public Button audioButton;
    public TextMeshProUGUI buttonText;
>>>>>>> tevin
    public AudioSource soundPlayer;

    // Start is called before the first frame update
    void Start()
    {
<<<<<<< HEAD
        // Set up the button listener
        audioButton.onClick.AddListener(OnButtonClick);   
    }

    // Handle button click
    public void OnButtonClick()
    {
        if (_isAudioPlaying)
        {
            _isAudioPlaying = false;
            soundPlayer.Pause();
            buttonText.text = "Audio Paused";
        }
        else
        {
            _isAudioPlaying = true;
            soundPlayer.Play();
            buttonText.text = "Audio is Playing";
        }
    }


=======
        audioButton.onClick.AddListener(OnButtonClick);
    }


    
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
<<<<<<< HEAD
>>>>>>> tevin
=======

    public void playThisAudio()
    {
        soundPlayer.Play();
    }

    public void pauseThisAudio()
    {
        soundPlayer.Pause();
    }
>>>>>>> tevin
}
