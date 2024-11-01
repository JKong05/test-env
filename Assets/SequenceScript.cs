using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using UnityEngine.XR.Hands.Samples.GestureSample;
using TMPro;
using System.Drawing;
using System;

public class SequenceScript : MonoBehaviour
{
    // To keep track of the current function
    private List<IEnumerator> sequentialSteps;
    private int currentStepIndex = 0;

    //public variables (able to access within Unity editor)
    [Header("Video")]
    public VideoPlayer videoPlayer;
    public Material videoMaterial;
    public VideoClip introClip;
    public List<VideoClip> videoClips;
    private bool isVideoPlaying = false;

    [Header("Audio")]
    public AudioSource soundPlayer;
    public List<AudioClip> audioClips;
    [Header("Mic")]
    public MicRecorder micRecorderObj;

    [Header("Fog")]
    public GameObject fogParent;
    private bool updateFogScale = false;
    private float fogElapsedTime = 0f;
    private Vector3 fogNewScale = new Vector3(1f, 1f, 1f);
    public float fogLerpDuration = 1f;
    public Vector3 fogSmallScale;
    public Vector3 fogLargeScale;
    private List<Transform> childTransforms = new List<Transform>();
    private List<float> fogChildOriginalSizes = new List<float>();
    public float fogChildScaleFactor;

    [Header("Environment")]
    public GameObject startEnvironment;

    public List<GameObject> environments;

    [Header("Gestures")]
    private string currentGesture = "";
    public GameObject rightThumbsUpDetector;
    public GameObject rightThumbsDownDetector;
    private StaticHandGesture thumbsUpGestureTracker;
    private StaticHandGesture thumbsDownGestureTracker;

    [Header("HUD Elements")]
    public TextMeshProUGUI ProgramStartText;
    public TextMeshProUGUI MicStatusText;
    public TextMeshProUGUI StoryModalityText;

    [Header("Testing Variables")]

    public TextMeshProUGUI gestureText;
    public int thumbsUpCount = 0;
    public TextMeshProUGUI functionCompleteText;
    public TextMeshProUGUI micActiveText;
    public TextMeshProUGUI storyTypeText;
    //0 = audio
    //1 = visual
    //2 = audiovisual
    public List<int> storyType = new List<int> { 0, 1, 2, 0, 1, 2 };
    public List<String> storyTitles;

    // Start is called before the first frame update
    void Start()
    {
        //Ensuring videoMaterial is assigned to videoObject
        if (videoMaterial != null && videoPlayer != null)
        {
            videoPlayer.GetComponent<Renderer>().material = videoMaterial;
        }

        foreach (Transform child in fogParent.transform)
        {
            childTransforms.Add(child);
            fogChildOriginalSizes.Add(child.localScale.x);
        }

        // Set the initial radius
        fogParent.transform.localScale = fogLargeScale;

        startEnvironment.SetActive(true);
        for (int i = 0; i < environments.Count; i++)
        {
            environments[i].SetActive(false);
        }

        //reset videplayer
        videoPlayer.targetTexture.Release();

        //setting gesture detectors
        thumbsUpGestureTracker = rightThumbsUpDetector.GetComponent<StaticHandGesture>();
        thumbsDownGestureTracker = rightThumbsDownDetector.GetComponent<StaticHandGesture>();

        if (thumbsUpGestureTracker != null)
        {
            thumbsUpGestureTracker.gesturePerformed.AddListener(OnThumbsUpPerformed);
        }
        else
        {
            Debug.LogError("Thumbs Up GestureTracker component not found on the assigned GameObject.");
        }

        if (thumbsDownGestureTracker != null)
        {
            thumbsDownGestureTracker.gesturePerformed.AddListener(OnThumbsDownPerformed);
        }
        else
        {
            Debug.LogError("Thumbs Down GestureTracker component not found on the assigned GameObject.");
        }

        // Initialize the steps to execute
        sequentialSteps = new List<IEnumerator>
        {
            ProgramStarting(),
            Introduction(),
            ShowStories(),
            ProgramEnding()
        };

        // Start the sequential process
        StartCoroutine(ExecuteSequentialSteps());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            OnThumbsUpPerformed();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            OnThumbsDownPerformed();
        }
        if (updateFogScale)
        {
            fogElapsedTime += Time.deltaTime;
            //converting to normalized time (0 to 1)
            float t;
            if (fogNewScale == fogLargeScale)
            {
                t = Mathf.Clamp01(fogElapsedTime / (fogLerpDuration * 240f * Time.deltaTime));
            }
            else
            {
                t = Mathf.Clamp01(fogElapsedTime / (fogLerpDuration * 12f * Time.deltaTime));
            }
            fogParent.transform.localScale = Vector3.Lerp(fogParent.transform.localScale, fogNewScale, t);

            if (t >= 1f)
            {
                updateFogScale = false;
                fogElapsedTime = 0f;
            }
        }
        Vector3 parentScale = fogParent.transform.localScale;
        for (int i = 0; i < childTransforms.Count; i++)
        {
            Transform child = childTransforms[i];
            float originalSize = fogChildOriginalSizes[i];
            if (child != null)
            {
                // Invert the parent's scale for the child
                child.localScale = new Vector3(
                    originalSize / (parentScale.x / fogLargeScale.x) * Mathf.Lerp(1.0f, fogChildScaleFactor, 6 - parentScale.x / fogLargeScale.x),
                    originalSize / (parentScale.y / fogLargeScale.y) * Mathf.Lerp(1.0f, fogChildScaleFactor, 6 - parentScale.y / fogLargeScale.y),
                    originalSize / (parentScale.z / fogLargeScale.z) * Mathf.Lerp(1.0f, fogChildScaleFactor, 6 - parentScale.z / fogLargeScale.z)
                );
            }
        }
    }

    private IEnumerator ExecuteSequentialSteps()
    {
        while (currentStepIndex < sequentialSteps.Count)
        {
            // Execute the current step
            yield return StartCoroutine(sequentialSteps[currentStepIndex]);
            currentStepIndex++;
        }
    }

    private IEnumerator ProgramStarting()
    {
        Debug.Log("Program Starting...");
        functionCompleteText.text = "ready";
        yield return WaitForGesture(new List<string> { "ThumbsUp" });
        ProgramStartText.gameObject.SetActive(false);
    }

    private IEnumerator Introduction()
    {
        StartVideo(introClip);
        Debug.Log("Introduction...");
        functionCompleteText.text = "Introduction...";
        Debug.Log((float)introClip.length);
        yield return new WaitForSeconds((float)introClip.length);
        functionCompleteText.text = "ready";
        yield return WaitForGesture(new List<string> { "ThumbsUp", "ThumbsDown" });
        // Respond based on the gesture received
        if (currentGesture == "ThumbsUp")
        {
            Debug.Log("Thumbs Up received in Introduction.");
        }
        else if (currentGesture == "ThumbsDown")
        {
            Debug.Log("Thumbs Down received, repeating Introduction.");
            yield return StartCoroutine(Introduction());
        }
        currentGesture = ""; // Reset for the next iteration
    }

    private IEnumerator ShowStories()
    {
        for (int i = 0; i < 6; i++)
        {
            yield return StartCoroutine(ShowStory(i));
            yield return StartCoroutine(MicStart());
            yield return StartCoroutine(MicEnd(i));
        }
        yield return new WaitForSeconds(0f);
    }

    private IEnumerator ShowStory(int iteration)
    {
        Debug.Log($"Showing Story Part {iteration + 1}...");
        functionCompleteText.text = $"Showing Story Part {iteration + 1}...";
        yield return StartCoroutine(EnableEnvironment(iteration));
        yield return new WaitForSeconds(0.5f);
        StoryModalityText.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        float storyDuration = 0f;
        if (videoClips[iteration] != null)
        {
            storyDuration = (float)videoClips[iteration].length;
            StartVideo(videoClips[iteration]);
        }
        else
        {
            Debug.Log("No video");
        }
        yield return new WaitForSeconds(storyDuration);
        functionCompleteText.text = "ready";
        yield return WaitForGesture(new List<string> { "ThumbsUp" });
    }

    private IEnumerator MicStart()
    {
        Debug.Log("Mic Starting...");
        functionCompleteText.text = "Mic Starting...";
        micActiveText.text = "Mic On";
        MicStatusText.gameObject.SetActive(true);
        micRecorderObj.GetComponent<MicRecorder>().StartRecording();
        yield return new WaitForSeconds(0.5f);
        functionCompleteText.text = "ready";
        yield return WaitForGesture(new List<string> { "ThumbsUp" });
    }

    private IEnumerator MicEnd(int iteration)
    {
        Debug.Log("Mic Ending...");
        functionCompleteText.text = "Mic Ending...";
        micActiveText.text = "Mic Off";
        micRecorderObj.GetComponent<MicRecorder>().StopRecording(iteration+1);
        MicStatusText.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
    }

    private IEnumerator ProgramEnding()
    {
        Debug.Log("Program Ending...");
        functionCompleteText.text = "Program Ending...";
        yield return new WaitForSeconds(2f); // Simulate some operation
    }

    void StartVideo(VideoClip videoClip)
    {
        // Stop current video
        videoPlayer.Stop();
        //Change to new video
        videoPlayer.clip = videoClip;
        //Play new video
        videoPlayer.Play();
    }

    void PauseVideo()
    {
        //Pause current video
        videoPlayer.Pause();
    }

    void ResumeVideo()
    {
        //Pause current video
        videoPlayer.Play();
    }

    void StartAudio(int index)
    {
        if (index >= 0 && index < audioClips.Count)
        {
            // Stop current audio
            soundPlayer.Stop();
            // Change to new audio clip
            soundPlayer.clip = audioClips[index];
            // Play new audio clip
            soundPlayer.Play();
        }
        else
        {
            Debug.LogWarning("Invalid audio index.");
        }
    }

    void playThisAudio()
    {
        soundPlayer.Play();
    }

    void pauseThisAudio()
    {
        soundPlayer.Pause();
    }

    void ExpandFog()
    {
        fogNewScale = fogLargeScale;
        fogElapsedTime = 0f;
        updateFogScale = true;
    }

    void ShrinkFog()
    {
        fogNewScale = fogSmallScale;
        fogElapsedTime = 0f;
        updateFogScale = true;
    }

    IEnumerator EnableEnvironment(int envNum)
    {
        ShrinkFog();
        yield return new WaitForSeconds(1.2f);
        String modality = storyType[envNum] == 0 ? "Audio" : storyType[envNum] == 1 ? "Video" : storyType[envNum] == 2 ? "Audiovisual" : "Unknown Type";
        String title = storyTitles[envNum];
        StoryModalityText.text = $"<size=100%>{title}<br><size=75%>{modality}";
        StoryModalityText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        videoPlayer.GetComponent<Renderer>().enabled = (storyType[envNum] != 0);
        videoPlayer.SetDirectAudioMute(0, (storyType[envNum] == 1));
        if (storyType[envNum] == 0)
        {
            storyTypeText.text = "Audio";
        }
        else if (storyType[envNum] == 1)
        {
            storyTypeText.text = "Video";
        }
        else if (storyType[envNum] == 2)
        {
            storyTypeText.text = "AudioVisual";
        }
        startEnvironment.SetActive(false);
        for (int i = 0; i < environments.Count; i++)
        {
            if (i == envNum)
            {
                environments[i].SetActive(true);
            }
            else
            {
                environments[i].SetActive(false);
            }
        }
        ExpandFog();
    }

    private IEnumerator WaitForGesture(List<string> validGestures)
    {
        currentGesture = ""; // Reset gesture
        // Wait until a gesture is detected that matches the valid gestures
        while (!validGestures.Contains(currentGesture))
        {
            yield return null; // Keep waiting until a valid gesture is set
        }
    }

    private void HandleGestureResponse()
    {
        switch (currentGesture)
        {
            case "ThumbsUp":
                Debug.Log("Thumbs Up received.");
                break;
            case "ThumbsDown":
                Debug.Log("Thumbs Down received.");
                break;
            default:
                Debug.Log("Unknown gesture.");
                break;
        }
        currentGesture = ""; // Reset for the next gesture
    }

    // Gesture detection methods
    public void OnThumbsUpPerformed()
    {
        currentGesture = "ThumbsUp"; // Set the current gesture to Thumbs Up
    }

    public void OnThumbsDownPerformed()
    {
        currentGesture = "ThumbsDown"; // Set the current gesture to Thumbs Down
    }


}
