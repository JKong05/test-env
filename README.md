# Environments Unity Project

```
├── .vscode
├── Assets
│   ├── Materials
│   ├── Objects
│   ├── OfficialAsset
│   ├── Packages
│   ├── Samples
│   ├── Scenes
│   ├── TextMesh Pro
│   ├── Video
│   ├── XR
│   ├── XRI
│   ├── facePlayer.cs                    # Makes videoplayer follow participant view (not used currently)
│   ├── HandGestureDetector.cs           # Hand gesture debugger tool (not used currently)
│   ├── HandTrackingVisualizer.cs        # Visualizes hand tracking data for debugging
│   ├── MicManager.cs                    # Used for debugging mic input
│   ├── MicRecorder.cs                   # Handles recording and saving mic audio
│   ├── NuGet.config
│   ├── packages.config
│   ├── SequenceScript.cs                # Controls the flow of stories and environments for participants
├── Packages
├── .gitignore
├── README.md
```

# Abstract
The research project explores how the story presentation modalities and environmental congruence affects the participant story retellings. The participants experience four stories in audio-only, visual-only, or audio and visual formats who then retell the stories to the best of their abilities in a VR program designed to be interacted with hand gestures. The audio recordings are then analyzed utilizing semantic vector analysis to compare patterns across participants and the large language model.

# Quick Start
1. Clone repository
```
git clone https://github.com/tevinpark/LLM_BJT_ENV.git
```
2. Open folder in Unity Editor
    - Unity will automatically install required packages
4. Run the project
    - Select main scene in Assets/Scenes/ folder
    - Click "Play" in Unity (top center play button)