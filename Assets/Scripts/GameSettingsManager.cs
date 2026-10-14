using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class GameSettingsManager : MonoBehaviour
{
    public AudioSource audioSource;
    public static GameSettingsManager Instance;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        audioSource = Camera.main.GetComponent<AudioSource>();
    }

    void Start()
    {
        Apply();
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
