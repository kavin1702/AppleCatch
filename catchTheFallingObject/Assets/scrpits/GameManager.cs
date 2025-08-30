
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton

    [Header("Game Settings")]
    public int score = 0;
    public float gameTime = 30f;
    private float currentTime;

    [Header("UI Elements")]
    public TMP_Text scoreText;
    public TMP_Text timerText;
    public GameObject gameOverPanel;
    public GameObject pauseMenuPanel;

    [Header("Panels")]
    public GameObject audioSettingsPanel; // Drag your AudioSettings panel here

    [Header("Audio")]
    public AudioMixer audioMixer;        // Main Audio Mixer
    public AudioSource backgroundAudio;  // Background music
    public AudioSource[] sfxAudios;      // Assign your SFX sources here
    public Slider musicSlider;           // UI slider for Music
    public Slider sfxSlider;             // UI slider for SFX

    private bool isPaused = false;

    void Awake()
    {
        // Singleton pattern
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 1;
        currentTime = gameTime;
        gameOverPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);

        // Init score
        scoreText.text = "Score : 0"; 

        // Load saved audio values (default 0.75f)
        float savedMusic = PlayerPrefs.GetFloat("MusicVol", 0.75f);
        float savedSFX = PlayerPrefs.GetFloat("SFXVol", 0.75f);

        if (musicSlider != null)
        {
            musicSlider.value = savedMusic;
            SetMusicVolume(savedMusic);
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            sfxSlider.value = savedSFX;
            SetSFXVolume(savedSFX);
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }
    }

    void Update()
    {
        // Countdown Timer
        currentTime -= Time.deltaTime;
        timerText.text = "" + Mathf.Ceil(currentTime);

        if (currentTime <= 0)
        {
            GameOver();
        }
    }

    // ================================
    // SCORE FUNCTIONS
    // ================================
    public void IncreaseScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
    }

    // ================================
    // GAME FLOW FUNCTIONS
    // ================================
    public void GameOver()
    {
        gameOverPanel.SetActive(true);

        // Stop background music
        if (backgroundAudio != null)
            backgroundAudio.Stop();

        Time.timeScale = 0; // Pause game
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main_Menu"); // Make sure scene is in Build Settings
    }

    public void PauseGame()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OpenAudioSettings()
    {
        audioSettingsPanel.SetActive(true);
    }

    public void CloseAudioSettings()
    {
        audioSettingsPanel.SetActive(false);
    }

    public void ShowPanelOfSlider()
    {
        pauseMenuPanel.SetActive(true);
    }

    // ================================
    // AUDIO FUNCTIONS
    // ================================
    public void SetMusicVolume(float value)
    {
        if (value <= 0.0001f) value = 0.0001f; // prevent log(0)
        audioMixer.SetFloat("MusicVolume ", Mathf.Log10(value) * 20); // dB scale
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        if (value <= 0.0001f) value = 0.0001f;
        audioMixer.SetFloat("SFX", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFX", value);
    }
}
