using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    public static HomeManager instance;
    public AudioClip selectClip;
    public GameObject settingCanvus;

    AudioSource audioSource;

    private void Start()
    {
        instance = this;
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

    public void OpenSetting()
    {
        PlayAudio(selectClip);
        settingCanvus.GetComponent<Canvas>().sortingOrder = 1;
    }

    public void CloseSetting()
    {
        PlayAudio(selectClip);
        settingCanvus.GetComponent<Canvas>().sortingOrder = -1;
    }

    public static void DestroyControl<T>(int len) where T : MonoBehaviour
    {
        T[] obj = FindObjectsOfType<T>();
        if (obj.Length == len)
        {
            for (int i = 0; i < len; i++)
                DontDestroyOnLoad(obj[i]);
        }
        else
        {
            for (int i = len; i < obj.Length; i++)
            {
                Destroy(obj[i]);
            }
        }
    }
}
