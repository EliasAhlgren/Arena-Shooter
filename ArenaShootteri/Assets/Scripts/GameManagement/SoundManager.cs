﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static AudioClip PLAYERHIT, ENEMYHIT, MenuClick, FootStep, Oof, Shoot, Reload, Empty, GateOpen, GateClose;

    static AudioSource audioSrc;
    public AudioMixer audioMixer;

    // Start is called before the first frame update
    void Start()
    {
        ENEMYHIT = Resources.Load<AudioClip>("Hitmarker");
        PLAYERHIT = Resources.Load<AudioClip>("Oof");
        MenuClick = Resources.Load<AudioClip>("ButtonClick1");
        FootStep = Resources.Load<AudioClip>("FootStep1");
        Oof = Resources.Load<AudioClip>("Oof");
        Shoot = Resources.Load<AudioClip>("PIT-AR-SHOT");
        Reload = Resources.Load<AudioClip>("PIT-AR-RELOAD");
        Empty = Resources.Load<AudioClip>("emptymag");
        GateOpen = Resources.Load<AudioClip>("GATE_Metal_Open_02_Dark_Short_stereo");
        GateClose = Resources.Load<AudioClip>("GATE_Metal_Close_02_Dark_stereo");

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
            case "GateOpen":
                audioSrc.Stop();
                audioSrc.loop = false;
                audioSrc.clip = GateOpen;
                audioSrc.Play();
                break;
            case "GateClose":
                audioSrc.Stop();
                audioSrc.loop = false;
                audioSrc.clip = GateClose;
                audioSrc.Play();
                break;
            case "Shoot":
                audioSrc.Stop();
                audioSrc.loop = false;
                audioSrc.clip = Shoot;
                audioSrc.pitch = Random.Range(1.0f - 0.01f, 1.0f + 0.01f);
                audioSrc.Play();
                break;
            case "Empty":
                audioSrc.Stop();
                audioSrc.loop = false;
                audioSrc.clip = Empty;
                audioSrc.Play();
                break;
            case "Reload":
                audioSrc.Stop();
                audioSrc.loop = false;
                audioSrc.clip = Reload;
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
                Debug.Log("WalkStep");
                audioSrc.pitch = Random.Range(0.9f - 0.05f, 0.9f + 0.05f);
                audioSrc.Play();
                break;
            case "Oof":
                audioSrc.Stop();
                audioSrc.loop = false;
                audioSrc.clip = Oof;
                audioSrc.Play();
                break;
        }
    }

}
