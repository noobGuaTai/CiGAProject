using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainPageManager : MonoBehaviour
{
    public GameObject gameSettingsPanel;
    public GameObject sliderBGM;
    public Dictionary<string, AudioClip> soundClip;
    public GameObject globalManager;
    public GameObject helpPanel;

    void Start()
    {
        helpPanel = transform.Find("HelpPanel").gameObject;
    }

    void Update()
    {

    }

    public void return_from_help() {
        helpPanel.SetActive(false);
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
        gameSettingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        gameSettingsPanel.SetActive(false);
    }

}
