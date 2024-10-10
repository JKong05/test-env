using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine.XR.Hands.Samples.GestureSample;

public class SequenceScript : MonoBehaviour
{
    //public variables (able to access within Unity editor)
    [Header("Video")] 
    public VideoPlayer videoPlayer;
    public Material videoMaterial;
    public List<VideoClip> videoClips;

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

        environments[0].SetActive(true);
        environments[1].SetActive(false);

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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TestAudioVideo();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PauseTestAudioVideo();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ResumeTestAudioVideo();
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            StartCoroutine(EnableEnvironment(1));
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            StartCoroutine(EnableEnvironment(2));
        }

        if (updateFogScale)
        {
            fogElapsedTime += Time.deltaTime;
            //converting to normalized time (0 to 1)
            float t;
            if(fogNewScale == fogLargeScale){
                t = Mathf.Clamp01(fogElapsedTime / (fogLerpDuration*80));
            }
            else{
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
                    originalSize / (parentScale.x / fogLargeScale.x) *  Mathf.Lerp(1.0f, fogChildScaleFactor, 6 - parentScale.x / fogLargeScale.x),
                    originalSize / (parentScale.y / fogLargeScale.y) * Mathf.Lerp(1.0f, fogChildScaleFactor, 6 - parentScale.y / fogLargeScale.y),
                    originalSize / (parentScale.z / fogLargeScale.z) * Mathf.Lerp(1.0f, fogChildScaleFactor, 6 - parentScale.z / fogLargeScale.z)
                );
            }
        }
    }

    void ProgramStarting()
    {

    }

    void Introduction()
    {

    }

    void ShowStory()
    {

    }

    void ProgramEnding()
    {

    }

    void TestAudioVideo(){
        StartVideo(0);
        playThisAudio();
    }

    void ResumeTestAudioVideo(){
        PauseVideo();
        playThisAudio();
    }

    void PauseTestAudioVideo(){
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
        Debug.Log("expand");
    }

    void ShrinkFog()
    {
        fogNewScale = fogSmallScale;
        fogElapsedTime = 0f;
        updateFogScale = true;
        Debug.Log("shrink");
    }

    IEnumerator EnableEnvironment(int envNum){
        if(envNum == 1){
            ShrinkFog();
            yield return new WaitForSeconds(5.0f);
            environments[0].SetActive(true);
            environments[1].SetActive(false);
            ExpandFog();
            Debug.Log("environment 1");
        }
        else if(envNum == 2){
            ShrinkFog();
            yield return new WaitForSeconds(5.0f);
            environments[0].SetActive(false);
            environments[1].SetActive(true);
            ExpandFog();
            Debug.Log("environment 2");
        }
    }

    void OnThumbsUpPerformed()
    {
        // This method will be called when the gesture is detected
        Debug.Log("Thumbs Up detected in listener!");
    }

}
