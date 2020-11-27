using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public AudioMixerGroup targetGroup;
    public SoundManager soundManager;
    public AudioMixer mixer;
    public Slider slider;

    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        slider.value = PlayerPrefs.GetFloat(targetGroup.name);
    }

    public void SetLevel(float sliderValue)
    {
        soundManager.SetVolume(targetGroup, sliderValue);
    }
}