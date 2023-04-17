using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsPanelSCR : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;
    [SerializeField] Toggle muteToggle;
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] TMP_Text bgmVolText;
    [SerializeField] TMP_Text sfxVolText;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown windowmodeDropdown;
    private int currentResolutionIndex;

    private void OnEnable()
    {
        muteToggle.isOn = audioManager.IsMuted;
        bgmSlider.value = audioManager.BGMVolume;
        sfxSlider.value = audioManager.SFXVolume;
        SetBGMVolText(bgmSlider.value);
        SetSFXVolText(sfxSlider.value);

        ResolutionEntries();
    }

    public void ResolutionEntries()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            string option = Screen.resolutions[i].width + " x " + Screen.resolutions[i].height;
            options.Add(option);
            if (Screen.resolutions[i].width == Screen.currentResolution.width &&
                               Screen.resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        currentResolutionIndex = PlayerPrefs.GetInt("Resolution", 0);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution()
    {
        Resolution resolution = Screen.resolutions[resolutionDropdown.value];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        resolutionDropdown.RefreshShownValue();

        PlayerPrefs.SetInt("Resolution", resolutionDropdown.value);
        PlayerPrefs.Save();
    }

    public void SetBGMVolText(float value)
    {
        bgmVolText.text = Mathf.RoundToInt(value * 100).ToString();
    }

    public void SetSFXVolText(float value)
    {
        sfxVolText.text = Mathf.RoundToInt(value * 100).ToString();
    }
}
