using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPageManager : MonoBehaviour
{
    public GameObject gameSettingsCanvas;
    public GameObject sliderBGM;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        audioSource.volume = sliderBGM.GetComponent<Slider>().value;
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
