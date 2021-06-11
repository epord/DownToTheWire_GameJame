using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionMenu : MonoBehaviour
{

    public const float DEFAULT_VOLUME = 0.75f;
    
    public AudioMixer AudioMixer;
    public Slider MusicSlider;
    public Slider SFXSlider;

    private void Start()
    {
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume", DEFAULT_VOLUME);
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume", DEFAULT_VOLUME);
    }

    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        setVolume("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", volume);
        setVolume("SFXVolume", volume);
    }

    private void setVolume(string name, float volume)
    {
        float newVolume = Mathf.Log10(volume) * 20;
        AudioMixer.SetFloat(name, newVolume);
    }
}
