using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    public GameObject playButton;
    public GameObject selectLevelButton;
    public GameObject settingButton;
    public GameObject exitButton;
    public GameObject menuPanel;
    public GameObject selectLevelPanel;
    public GameObject settingsPanel;
    public GameObject buttonsPanel;
    public GameObject buttonPrefab;
    public Slider volumeSlider;
    public Dropdown resolutionDropdown;
    public Toggle toggleFullscreen;
    public GameSettingsManager gameSettingsManager;
    private List<Dropdown.OptionData> resolutionList = new List<Dropdown.OptionData>();

    void Start()
    {
        playButton = GameObject.Find("Play 1 level button");
        selectLevelButton = GameObject.Find("Choose level button");
        settingButton = GameObject.Find("Settings button");
        exitButton = GameObject.Find("Exit button");
        menuPanel = GameObject.Find("Menu Panel");
        foreach (Resolution res in Screen.resolutions)
        {
            Dropdown.OptionData option = new Dropdown.OptionData(res.width + " x " + res.height);
            resolutionList.Add(option);
        }
        resolutionDropdown.options = resolutionList;
        GenerateLevelButtons();
        LoadUI();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void SwitchToLevelSelect()
    {
        menuPanel.SetActive(false);
        selectLevelPanel.SetActive(true);
    }

    public void Settings()
    {
        menuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }


    public void SaveAndApply()
    {
        PlayerPrefs.SetInt("ResIndex", resolutionDropdown.value);
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.SetInt("Fullscreen", toggleFullscreen.isOn ? 1 : 0);
        PlayerPrefs.Save();
        gameSettingsManager.Apply();
        GoBackToMenu();
    }

    public void ExitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    public void GoBackToMenu()
    {
        menuPanel.SetActive(true);
        selectLevelPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void ChooseLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    void LoadUI()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume, 1f");
        resolutionDropdown.value = PlayerPrefs.GetInt("ResIndex", 0);
        toggleFullscreen.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
    }

    void GenerateLevelButtons()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings - 1;
        for (int i = 1; i <= sceneCount; i++)
        {
            int currentIndex = i;
            GameObject button = Instantiate(buttonPrefab, buttonsPanel.transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = "Level " + i.ToString();
            Button btn = button.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => ChooseLevel(currentIndex));
        }
    }
}
