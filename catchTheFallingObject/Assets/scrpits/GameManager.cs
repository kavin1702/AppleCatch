
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public int score = 0;
    public float gameTime = 30f;
    private float currentTime;

    [Header("UI Elements")]
    public TMP_Text timerText;
    public GameObject gameOverPanel;
    public GameObject pauseMenuPanel;
    [SerializeField] private AudioMixer audioMixer;
    [Header("Panels")]
    public GameObject audioSettingsPanel; // Drag your AudioSettings panel here

    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Audio")]
    public AudioSource backgroundAudio; // background music
    public AudioSource[] sfxAudios;     // assign your SFX AudioSources here

    private bool isPaused = false;

    void Start()
    {
        Time.timeScale = 1;
        currentTime = gameTime;
        gameOverPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);

        // Load saved values (or default 0.75f if none exist)
        float savedMusic = PlayerPrefs.GetFloat("MusicVol", 0.75f);
        float savedSFX = PlayerPrefs.GetFloat("SFXVol", 0.75f);

        musicSlider.value = savedMusic;
        sfxSlider.value = savedSFX;

        SetMusicVolume(savedMusic);
        SetSFXVolume(savedSFX);

        // Add listeners AFTER setting initial values
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }


    void Update()
    {
     
        currentTime -= Time.deltaTime; 
        timerText.text = " " + Mathf.Ceil(currentTime);

        if (currentTime <= 0)
        {
            GameOver();
        }
    }


    public void GameOver()
    {
        gameOverPanel.SetActive(true);

        // Stop background music
        if (backgroundAudio != null)
            backgroundAudio.Stop();

        Time.timeScale = 0; // Pause the game
    }


    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main_Menu"); // make sure MainMenu is in Build Settings
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
        pauseMenuPanel. SetActive(true);
    }
    public void SetMusicVolume(float value)
    {
        if (value <= 0.0001f) value = 0.0001f; // prevent log(0) problem
        audioMixer.SetFloat("Music", Mathf.Log10(value) * 20); // dB scale
        PlayerPrefs.SetFloat("MusicVol", value); // save
    }

    public void SetSFXVolume(float value)
    {
        if (value <= 0.0001f) value = 0.0001f;
        audioMixer.SetFloat("SFX", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFXVol", value);
    }



}
