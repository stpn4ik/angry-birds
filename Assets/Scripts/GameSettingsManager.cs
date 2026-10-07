using UnityEngine;

public class GameSettingsManager : MonoBehaviour
{
    public AudioSource audioSource;

    void Awake()
    {
        Apply();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        audioSource = Camera.main.GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    public void Apply()
    {
        audioSource.volume = PlayerPrefs.GetFloat("Volume", 1f);
        int resIndex = PlayerPrefs.GetInt("ResIndex", 0);
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        var res = Screen.resolutions[resIndex];
        Screen.SetResolution(res.width, res.height, isFullscreen);
    }
    
}
