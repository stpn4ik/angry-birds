using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] birds;
    public CameraController cam;
    private int birdIndex;
    private GameObject currentBird;
    public float reloadBirdDelay = 3f;
    private ThrowBird throwBird;
    public TextMeshProUGUI numText;
    public GameObject pauseMenu;
    public GameObject UIMenu;
    public GameObject settingsMenu;
    public AudioSource audioSource;
    public Slider volumeSlider;

    void Start()
    {
        birdIndex = 0;
        LoadBird();
        numText.text = SceneManager.sceneCount.ToString();
    }

    void Update()
    {

    }

    void LoadBird()
    {
        currentBird = birds[birdIndex];
        cam.bird = currentBird;
        throwBird = currentBird.GetComponent<ThrowBird>();
        currentBird.transform.position = throwBird.startPosition.position;
        throwBird.InQueue(false);
        currentBird.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        throwBird.onStopped += HandleBirdStopped;
    }

    void HandleBirdStopped()
    {
        throwBird.onStopped -= HandleBirdStopped;
        StartCoroutine(ReloadNextBird());
    }

    IEnumerator ReloadNextBird()
    {
        yield return new WaitForSeconds(reloadBirdDelay);
        currentBird.SetActive(false);
        if (birdIndex < birds.Length - 1)
        {
            birdIndex += 1;
            LoadBird();
        }
        else
            Debug.Log("Level over");
    }

    public void StartScene()
    {
        print("Game started");
    }

    public void OpenPauseMenu()
    {
        UIMenu.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void GoBackToGame()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        UIMenu.SetActive(true);
        Time.timeScale = 1f;
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Settings()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void GoBackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void GoBackToPauseMenu()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void SaveAndApplySettings()
    {
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        audioSource.volume = PlayerPrefs.GetFloat("Volume", 1f);
    }

}
