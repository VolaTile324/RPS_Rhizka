using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    static AudioSource bgmInst;
    static AudioSource sfxInst;
    [SerializeField] AudioSource bgm;
    [SerializeField] AudioSource sfx;

    public bool IsMuted { get => bgm.mute; }
    public float BGMVolume { get => bgm.volume; }
    public float SFXVolume { get => sfx.volume; }

    private void Awake()
    {
        // load from player prefs
        bgm.mute = PlayerPrefs.GetInt("Mute", 0) == 1;
        bgm.volume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfx.volume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        if(bgmInst != null)
        {
            Destroy(bgm.gameObject);
            bgm = bgmInst;
        }
        else
        {
            bgmInst = bgm;
            bgm.transform.SetParent(null);
            DontDestroyOnLoad(bgm.gameObject);
        }

        if (sfxInst != null)
        {
            Destroy(sfx.gameObject);
            sfx = sfxInst;
        }
        else
        {
            sfxInst = sfx;
            sfx.transform.SetParent(null);
            DontDestroyOnLoad(sfx.gameObject);
        }
    }

    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (bgm.isPlaying)
        {
            bgm.Stop();
        }
        
        bgm.clip = clip;
        bgm.loop = loop;
        bgm.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfx.isPlaying)
        {
            sfx.Stop();
        }
        
        sfx.clip = clip;
        sfx.Play();
    }

    public void SetMute(bool value)
    {
        bgm.mute = value;
        sfx.mute = value;
        // save to player prefs
        PlayerPrefs.SetInt("Mute", value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SetBGMVolume(float value)
    {
        bgm.volume = value;
        // save to player prefs
        PlayerPrefs.SetFloat("BGMVolume", value);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float value)
    { 
        sfx.volume = value;
        // save to player prefs
        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
    }
}
