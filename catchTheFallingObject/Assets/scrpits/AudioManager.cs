using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Mixer")]
    public AudioMixer audioMixer;

    [Header("Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        if (musicSlider != null)
        {
            float musicVol = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
            musicSlider.value = musicVol;
            SetMusicVolume(musicVol);
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }

        if (sfxSlider != null)
        {
            float sfxVol = PlayerPrefs.GetFloat("SfxVolume", 0.75f);
            sfxSlider.value = sfxVol;
            SetSfxVolume(sfxVol);
            sfxSlider.onValueChanged.AddListener(SetSfxVolume);
        }
    }

    public void SetMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume ", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MusicVolume ", value);
    }

    public void SetSfxVolume(float value)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFX", value);
    }
}
