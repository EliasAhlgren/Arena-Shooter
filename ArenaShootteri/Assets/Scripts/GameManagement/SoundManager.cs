using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static AudioClip PLAYERHIT, ENEMYHIT, MenuClick, FootStep;

    static AudioSource audioSrc;
    public AudioMixer audioMixer;

    // Start is called before the first frame update
    void Start()
    {
        ENEMYHIT = Resources.Load<AudioClip>("Hitmarker");
        PLAYERHIT = Resources.Load<AudioClip>("Oof");
        MenuClick = Resources.Load<AudioClip>("ButtonClick1");
        FootStep = Resources.Load<AudioClip>("FootStep1");

        audioSrc = GetComponent<AudioSource>();
        audioMixer.SetFloat("Sound", Mathf.Log10(PlayerPrefs.GetFloat("Sound")) * 20);
        audioMixer.SetFloat("Music", Mathf.Log10(PlayerPrefs.GetFloat("Music")) * 20);
    }

    public void SetVolume(AudioMixerGroup targetGroup, float value)
    {
        audioMixer.SetFloat(targetGroup.name, Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat(targetGroup.name, value);
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "ENEMYHIT":
                audioSrc.Stop();
                audioSrc.loop = true;
                audioSrc.clip = ENEMYHIT;
                audioSrc.Play();
                break;
            case "PLAYERHIT":
                audioSrc.Stop();
                audioSrc.loop = false;
                audioSrc.clip = PLAYERHIT;
                audioSrc.Play();
                break;
            case "MenuClick":
                audioSrc.Stop();
                audioSrc.loop = false;
                audioSrc.clip = MenuClick;
                audioSrc.Play();
                break;
            case "WalkStep":
                audioSrc.Stop();
                audioSrc.loop = false;
                audioSrc.clip = FootStep;
                audioSrc.pitch = Random.Range(1.0f - 0.15f, 1.0f + 0.15f);
                audioSrc.Play();
                break;
        }
    }

}
