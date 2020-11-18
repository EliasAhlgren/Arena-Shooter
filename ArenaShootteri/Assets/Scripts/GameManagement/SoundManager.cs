using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static AudioClip PLAYERHIT, ENEMYHIT;

    static AudioSource audioSrc;
    public AudioMixer audioMixer;

    // Start is called before the first frame update
    void Start()
    {
        ENEMYHIT = Resources.Load<AudioClip>("Hitmarker");
        PLAYERHIT = Resources.Load<AudioClip>("Oof");

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
        }
    }

}
