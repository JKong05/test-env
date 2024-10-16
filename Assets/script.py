from openai import OpenAI
from pathlib import Path
from pydub import AudioSegment
from dotenv import load_dotenv
import os

env_path = Path(__file__).parent / ".env" # can delete line 7 and 8
load_dotenv(dotenv_path=env_path)

client = OpenAI(api_key = os.getenv("API_KEY")) # need to check openAI to get the API key and replace API_KEY with actual one

output_dir = Path(__file__).parent / "audio_chunks"
output_dir.mkdir(exist_ok=True)
with open("/Users/justink_5/Desktop/test-env/test-env/story.txt", "r") as file: # replace the file directory with your stuff
    content = file.read()

input_text = content

# Function to split the text into chunks
def split_text(text, max_length=4096):
    return [text[i:i+max_length] for i in range(0, len(text), max_length)]

chunks = split_text(input_text)

# Loop through the chunks and send each as a separate request
for idx, chunk in enumerate(chunks):
    response = client.audio.speech.create(
        model="tts-1",  
        voice="nova",  
        input=chunk
    )

    speech_file_path = output_dir / f"speech_part{idx + 1}.mp3"

    response.stream_to_file(speech_file_path)

audio_files = sorted(audio_dir.glob("*.mp3"))


combined_audio = AudioSegment.empty()

for audio_file in audio_files:
    audio = AudioSegment.from_mp3(audio_file)
    combined_audio += audio

output_file = audio_dir / "combined_audio.mp3"
combined_audio.export(output_file, format="mp3")

print(f"All audio files have been merged and saved as {output_file}")