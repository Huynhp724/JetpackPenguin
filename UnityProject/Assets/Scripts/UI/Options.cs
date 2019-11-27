﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Options : MonoBehaviour
{
    [SerializeField] WorldManager wm;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Toggle buttonToggle;
    [SerializeField] AudioMixer mixer;
    [SerializeField] GameObject buttons;


    void GetOptions(float masterVol, float musicVol, float sfxVol, bool buttonPrompts)
    {
        masterSlider.value = masterVol;
        musicSlider.value = musicVol;
        sfxSlider.value = sfxVol;
        buttonToggle.isOn = buttonPrompts;
    }

    void Start()
    {
        if (wm == null)
        {
            wm = FindObjectOfType<WorldManager>();
        }
        wm.updateOptions += GetOptions;
        wm.callGetOptions();
    }

    public void saveOptions()
    {
        wm.setOptions(masterSlider.value, musicSlider.value, sfxSlider.value, buttonToggle.isOn);
    }

    public void adjustMasterVolume(float value)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
        saveOptions();
    }

    public void adjustSFXVolume(float value)
    {
        mixer.SetFloat("PluckVolume", Mathf.Log10(value) * 20);
        mixer.SetFloat("InteractableVolume", Mathf.Log10(value) * 20);
        mixer.SetFloat("EnvironmentVolume", Mathf.Log10(value) * 20);
        saveOptions();
    }

    public void toggleButtons(bool isVisible)
    {
        if(buttons != null)
            buttons.SetActive(isVisible);
        saveOptions();
    }

    public void adjustMusicVolume(float value)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        saveOptions();
    }

}
