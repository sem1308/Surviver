using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public string[] soundTypes = { "Master", "BGM", "SFX" };
    public int numSound = 3; // 전체 slider 수

    public AudioMixer audioMixer; // Audio Mixer
    public Slider[] audioSliders; // Audio Sliders

    private void Awake()
    {
        // audioMixer value를 slider에 적용
        for (int i = 0; i < numSound; i++)
        {
            float value;
            audioMixer.GetFloat(soundTypes[i], out value);
            audioSliders[i].value = value;
        }
    }

    public void AudioControl(int soundIdx)
    {
        float value = audioSliders[soundIdx].value;
        if (value == -40f) audioMixer.SetFloat(soundTypes[soundIdx], -80); // -80은 음소거를 위함
        else audioMixer.SetFloat(soundTypes[soundIdx], value);

        // HomeManager.instance.PlayAudio(HomeManager.instance.selectClip);
    }

    public void ToggleAudioVolume()
    {
        AudioListener.volume = AudioListener.volume == 0 ? 1 : 0;
    }
}
