using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPageManager : MonoBehaviour
{
    public GameObject gameSettingsCanvas;
    public GameObject sliderBGM;
    public Dictionary<string, AudioClip> soundClip;
    public GameObject globalManager;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        StartCoroutine(LoadBGM());
    }

    void Update()
    {
        audioSource.volume = sliderBGM.GetComponent<Slider>().value;
    }

    IEnumerator LoadBGM()
    {
        yield return null;
        soundClip = new Dictionary<string, AudioClip>();
        AudioClip[] clips = Resources.LoadAll<AudioClip>("sound/BackgroundMusic");

        foreach (AudioClip clip in clips)
        {
            if (!soundClip.ContainsKey(clip.name))
            {
                soundClip[clip.name] = clip;
            }
            else
            {
                Debug.LogWarning("Duplicate effect prefab name found in 'sound/BackgroundMusic': " + clip.name);
            }
        }
    }

    public void ChangeBGM(string clipName)
    {
        audioSource.clip = soundClip[clipName];
        audioSource.Play();
    }

    public void StartGame()
    {
        gameObject.SetActive(false);
        globalManager.GetComponent<GlobalManager>().StartGame();
    }

    public void QuitGame()
    {
        // 如果是在编辑器中
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 如果是在构建后的游戏中
        Application.Quit();
#endif
    }

    public void OpenSettings()
    {
        gameSettingsCanvas.SetActive(true);
    }

    public void CloseSettings()
    {
        gameSettingsCanvas.SetActive(false);
    }

    public void PlaySound()
    {
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
    }

    public void StopSound()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    public void PauseSound()
    {
        if (audioSource != null)
        {
            audioSource.Pause();
        }
    }
}
