﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class PlayerSettings : MonoBehaviour
{
    [SerializeField] Slider volumeSlider, fovSlider, musicVolumeSlider, sfxVolumeSlider, mouseSensitivitySlider;
    [SerializeField] TMPro.TMP_Dropdown resolutionSelection;
    [SerializeField] Toggle fullScreen;
    [SerializeField] TMP_Text volumeText, fovText, musicVolumeText, sfxVolumeText, mouseSensitivityText;
    [SerializeField] TMP_InputField nickName;

    public Resolution[] resolutions;
    Dictionary<int, Resolution> resolutionDict = new Dictionary<int, Resolution>();
    private List<string> WidthByHeight = new List<string>();
    int curRes;

    public void Awake()
    {
        resolutions = Screen.resolutions;
        int highestRefresh = 0;
        foreach (Resolution r in resolutions)
        {
            if (r.refreshRate > highestRefresh)
                highestRefresh = r.refreshRate;
        }
        foreach (Resolution r in resolutions)
        {
            if (r.refreshRate == highestRefresh)
            {
                WidthByHeight.Add(r.width + "x" + r.height);
                resolutionDict.Add(resolutionDict.Count, r);
            }
        }
        loadSettings();
    }

    public void loadSettings()
    {
        volumeText.text = "Volume: " + boot.bootObject.currentSettings.masterVolume;
        volumeSlider.value = boot.bootObject.currentSettings.masterVolume;
        musicVolumeText.text = "Music Volume: " + boot.bootObject.currentSettings.musicVolume;
        musicVolumeSlider.value = boot.bootObject.currentSettings.musicVolume;
        sfxVolumeText.text = "SFX Volume: " + boot.bootObject.currentSettings.sfxVolume;
        sfxVolumeSlider.value = boot.bootObject.currentSettings.sfxVolume;
        fovText.text = "FOV: " + boot.bootObject.currentSettings.fov;
        fovSlider.value = boot.bootObject.currentSettings.fov;
        mouseSensitivityText.text = "Mouse Sensitivity: " + boot.bootObject.currentSettings.mouseSensitvity;
        mouseSensitivitySlider.value = boot.bootObject.currentSettings.mouseSensitvity;
        resolutionSelection.ClearOptions();
        resolutionSelection.AddOptions(WidthByHeight);
        foreach(var item in resolutionDict)
        {
            if ((item.Value.width == boot.bootObject.currentSettings.resolutionWidth) && (item.Value.height == boot.bootObject.currentSettings.resolutionHeight))
            {
                curRes = item.Key;
            }
        }
        resolutionSelection.value = curRes;
        fullScreen.SetIsOnWithoutNotify(boot.bootObject.currentSettings.fullscreen);
        nickName.text = boot.bootObject.currentSettings.nickname;
    }

    public void applySettings()
    {
        Screen.SetResolution(resolutionDict[resolutionSelection.value].width, resolutionDict[resolutionSelection.value].height, fullScreen.isOn);
        GameEvents.current.onNewSettingsEvent();
    }

    public void saveSettings()
    {
        PlayerInfo newSave = new PlayerInfo();
        newSave.masterVolume = (int)volumeSlider.value;
        newSave.fov = (int)fovSlider.value;
        newSave.savedResolution = resolutionDict[resolutionSelection.value];
        newSave.resolutionHeight = resolutionDict[resolutionSelection.value].height;
        newSave.resolutionWidth = resolutionDict[resolutionSelection.value].width;
        newSave.fullscreen = fullScreen.isOn;
        newSave.nickname = nickName.text;
        DataSaver.saveData(newSave, "config");
    }

    public void updateText()
    {
        volumeText.text = "Master Volume: " + volumeSlider.value;
        fovText.text = "FOV: " + fovSlider.value;
        musicVolumeText.text = "Music Volume: " + musicVolumeSlider.value;
        sfxVolumeText.text = "SFX Volume: " + sfxVolumeSlider.value;
        mouseSensitivityText.text = "Mouse Sensitivity: " + mouseSensitivitySlider.value;
    }
}
