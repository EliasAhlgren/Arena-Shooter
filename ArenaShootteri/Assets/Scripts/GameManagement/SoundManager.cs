using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static AudioClip PLAYERHIT, ENEMYHIT, MenuClick, ImpHit, vipHit, ImpDie, vipDie, FootStep, Oof, Shoot, Reload, Empty, RocketShot, GateOpen, GrenadeReload, GateClose, ShotGunShoot, GrenadeShoot, ShotGunReload;

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
        ShotGunShoot = Resources.Load<AudioClip>("PIT-SG-SHOT");
        ShotGunReload = Resources.Load<AudioClip>("PIT-SG-RELOAD");
        GrenadeShoot = Resources.Load<AudioClip>("GrenadeShoot");
        GrenadeReload = Resources.Load<AudioClip>("PIT-SG-RELOAD");
        RocketShot = Resources.Load<AudioClip>("rocketShort");
        ImpDie = Resources.Load<AudioClip>("PIT-MONSTERGROWL-3");
        ImpHit = Resources.Load<AudioClip>("PIT-MONSTERGROWL-1");
        vipHit = Resources.Load<AudioClip>("vipeltaja1");
        vipDie = Resources.Load<AudioClip>("vipeltaja2");

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
            case "vipDie":
                audioSrc.Stop();
                audioSrc.loop = false;
                audioSrc.clip = vipDie;
                audioSrc.Play();
                break;
            case "vipHit":
                audioSrc.Stop();
                audioSrc.loop = false;
                audioSrc.clip = vipHit;
                audioSrc.Play();
                break;
            case "ImpDie":
                audioSrc.Stop();
                audioSrc.loop = false;
                audioSrc.clip = ImpDie;
                audioSrc.Play();
                break;
            case "ImpHit":
                audioSrc.Stop();
                audioSrc.loop = false;
                audioSrc.clip = ImpHit;
                audioSrc.Play();
                break;
            case "RocketShot":
                audioSrc.Stop();
                audioSrc.loop = false;
                audioSrc.clip = RocketShot;
                audioSrc.Play();
                break;
            case "ShotGunShoot":
                audioSrc.Stop();
                audioSrc.loop = false;
                audioSrc.clip = ShotGunShoot;
                audioSrc.Play();
                break;
            case "ShotGunReload":
                audioSrc.Stop();
                audioSrc.loop = false;
                audioSrc.clip = ShotGunReload;
                audioSrc.Play();
                break;
            case "GrenadeShoot":
                audioSrc.Stop();
                audioSrc.loop = false;
                audioSrc.clip = GrenadeShoot;
                audioSrc.Play();
                break;
            case "GrenadeReload":
                audioSrc.Stop();
                audioSrc.loop = false;
                audioSrc.clip = GrenadeReload;
                audioSrc.Play();
                break;
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
//                Debug.Log("WalkStep");
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
