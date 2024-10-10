using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine.XR.Hands.Samples.GestureSample;

public class SequenceScript : MonoBehaviour
{
    // To keep track of the current function
    private int currentFunctionIndex = 0;
    private bool isExecuting = false;
    private bool thumbsUpReceived = false;

    //public variables (able to access within Unity editor)
    [Header("Video")]
    public VideoPlayer videoPlayer;
    public Material videoMaterial;
    public List<VideoClip> videoClips;
    private bool isVideoPlaying = false;

    [Header("Audio")]
    public AudioSource soundPlayer;

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
    public GameObject rightThumbsUpDetector;
    private StaticHandGesture gestureTracker;


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

        //setting gesture detectors
        gestureTracker = rightThumbsUpDetector.GetComponent<StaticHandGesture>();
        if (gestureTracker != null)
        {
            // Subscribe to the gesturePerformed event
            gestureTracker.gesturePerformed.AddListener(OnThumbsUpPerformed);
        }
        else
        {
            Debug.LogError("GestureTracker component not found on the assigned GameObject.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Alpha1))
        // {
        //     TestAudioVideo();
        // }
        // if (Input.GetKeyDown(KeyCode.Alpha2))
        // {
        //     PauseTestAudioVideo();
        // }
        // if (Input.GetKeyDown(KeyCode.Alpha3))
        // {
        //     ResumeTestAudioVideo();
        // }
        // if (Input.GetKeyDown(KeyCode.Alpha9))
        // {
        //     StartCoroutine(EnableEnvironment(0));
        // }
        // if (Input.GetKeyDown(KeyCode.Alpha0))
        // {
        //     StartCoroutine(EnableEnvironment(1));
        // }

        if (updateFogScale)
        {
            fogElapsedTime += Time.deltaTime;
            //converting to normalized time (0 to 1)
            float t;
            if (fogNewScale == fogLargeScale)
            {
                t = Mathf.Clamp01(fogElapsedTime / (fogLerpDuration * 80));
            }
            else
            {
                t = Mathf.Clamp01(fogElapsedTime / fogLerpDuration);
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

    private IEnumerator ExecuteCurrentFunction()
    {
        isExecuting = true; // Set flag to true, indicating the coroutine is running

        switch (currentFunctionIndex)
        {
            case 0:
                yield return StartCoroutine(ProgramStarting());
                break;
            case 1:
                yield return StartCoroutine(Introduction());
                break;
            case 2:
                for (int i = 0; i < 6; i++)
                {
                    yield return StartCoroutine(ShowStory(i));
                    yield return StartCoroutine(WaitForThumbsUpToStartMic());
                    yield return StartCoroutine(MicStart());
                    yield return StartCoroutine(WaitForThumbsUpToEndMic());
                    yield return StartCoroutine(MicEnd());
                }
                break;
            case 3:
                yield return StartCoroutine(ProgramEnding());
                break;
            default:
                Debug.Log("All functions executed.");
                currentFunctionIndex = 0; // Reset to start over
                yield break; // Exit coroutine
        }

        // Move to the next function
        currentFunctionIndex++;

        isExecuting = false; // Set flag back to false, indicating the coroutine has finished
    }

    private IEnumerator ProgramStarting()
    {
        Debug.Log("Program Starting...");
        yield return new WaitForSeconds(2f); // Simulate some operation
    }

    private IEnumerator Introduction()
    {
        Debug.Log("Introduction...");
        yield return new WaitForSeconds(2f); // Simulate some operation
    }

    private IEnumerator ShowStory(int iteration)
    {
        Debug.Log($"Showing Story Part {iteration + 1}...");
        yield return StartCoroutine(EnableEnvironment(iteration));
        yield return new WaitForSeconds(2f); // Simulate story duration
    }

    private IEnumerator WaitForThumbsUpToStartMic()
    {
        thumbsUpReceived = false; // Reset before waiting
        Debug.Log("Waiting for thumbs up to start the mic...");
        yield return new WaitUntil(() => thumbsUpReceived); // Wait until thumbs up is received
    }

    private IEnumerator MicStart()
    {
        Debug.Log("Mic Starting...");
        yield return new WaitForSeconds(1f); // Simulate mic start
    }

    private IEnumerator WaitForThumbsUpToEndMic()
    {
        thumbsUpReceived = false; // Reset before waiting
        Debug.Log("Waiting for thumbs up to end the mic...");
        yield return new WaitUntil(() => thumbsUpReceived); // Wait until thumbs up is received
    }

    private IEnumerator MicEnd()
    {
        Debug.Log("Mic Ending...");
        yield return new WaitForSeconds(1f); // Simulate mic end
    }

    private IEnumerator ProgramEnding()
    {
        Debug.Log("Program Ending...");
        yield return new WaitForSeconds(2f); // Simulate some operation
    }

    void TestAudioVideo()
    {
        StartVideo(0);
        playThisAudio();
    }

    void ResumeTestAudioVideo()
    {
        PauseVideo();
        playThisAudio();
    }

    void PauseTestAudioVideo()
    {
        PauseVideo();
        pauseThisAudio();
    }


    void StartVideo(int index)
    {
        if (index >= 0 && index < videoClips.Count)
        {
            // Stop current video
            videoPlayer.Stop();
            //Change to new video
            videoPlayer.clip = videoClips[index];
            //Play new video
            videoPlayer.Play();
        }
        else
        {
            Debug.LogWarning("Invalid video index.");
        }
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
        yield return new WaitForSeconds(2.0f);
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

    void OnThumbsUpPerformed()
    {
        // This method will be called when the gesture is detected
        // Debug.Log("Thumbs Up detected in listener!");
        if (!isExecuting) // Check if not already executing
        {
            StartCoroutine(ExecuteCurrentFunction());
        }
        thumbsUpReceived = true;
    }


}
