using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score = 0;
    public float gameTime = 30f;
    private float currentTime;

    public TMP_Text scoreText;
    public TMP_Text timerText;
    public GameObject gameOverPanel;
    public GameObject pauseMenuPanel;

    public GameObject audioSettingsPanel;

    public AudioMixer audioMixer;
    public AudioSource backgroundAudio;
    public AudioSource[] sfxAudios;
    public Slider musicSlider;
    public Slider sfxSlider;

    private bool isPaused = false;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Time.timeScale = 1;
        currentTime = gameTime;
        gameOverPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);

        scoreText.text = "Score : 0";

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
        currentTime -= Time.deltaTime;
        timerText.text = "" + Mathf.Ceil(currentTime);

        if (currentTime <= 0)
        {
            GameOver();
        }
    }

    public void IncreaseScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);

        if (backgroundAudio != null)
            backgroundAudio.Stop();

        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main_Menu");
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

    public void SetMusicVolume(float value)
    {
        if (value <= 0.0001f) value = 0.0001f;
        audioMixer.SetFloat("MusicVolume ", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        if (value <= 0.0001f) value = 0.0001f;
        audioMixer.SetFloat("SFX", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFX", value);
    }
}
