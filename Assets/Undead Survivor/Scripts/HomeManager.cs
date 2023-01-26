using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    public static HomeManager instance;
    public AudioClip selectClip;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void GameStart()
    {
        PlayAudio(selectClip);
        SceneManager.LoadScene("GameScene");
    }

    public void PlayAudio(AudioClip clip, float vol = 0.8f, float pitch = 1f)
    {
        audioSource.clip = clip;
        audioSource.volume = vol;
        audioSource.pitch = pitch;
        audioSource.Play();
    }
}
