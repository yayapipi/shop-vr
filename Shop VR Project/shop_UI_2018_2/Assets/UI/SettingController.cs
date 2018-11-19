using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour {
    public Text QualityText = null;
    public static int CurrentQuality = 0;

    public Text AntiStropicText = null;
    public static int CurrentAS = 0;

    public Text AntiAliasingText = null;
    public static int CurrentAA = 0;
    private string[] AAOptions = new string[] { "X0", "X2", "X4", "X8" };

    public Text vSyncText = null;
    public static int CurrentVSC = 0;
    private string[] VSCOptions = new string[] { "Don't Sync", "Every V Blank", "Every Second V Blank" };

    public Text blendWeightsText = null;
    public static int CurrentBW = 0;

    public Text helmetText = null;
    public static int CurrentHelmet = 0;

    public static float MainVolume;
    public static float MusicVolume;
    public static float SoundVolume;

    public Slider MainVolumeSlider = null;
    public Slider MusicVolumeSlider = null;
    public Slider SoundVolumeSlider = null;

    private static SettingController _instance = null;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        GetInfoOptions();
    }
    
    public static SettingController Instance()
    {
        if (_instance == null)
        {
            throw new Exception("could not find the SettingController object.");
        }
        return _instance;
    }

    public void Close()
    {
        MainController.Instance("SettingController").CloseUI();
        Destroy(transform.parent.gameObject);
    }

    void OnDestroy()
    {
        _instance = null;
    }

    public void GameQuality(bool mas)
    {
        if (mas)
        {
            CurrentQuality = (CurrentQuality + 1) % QualitySettings.names.Length;
        }
        else
        {
            if (CurrentQuality != 0)
            {
                CurrentQuality = (CurrentQuality - 1) % QualitySettings.names.Length;
            }
            else
            {
                CurrentQuality = (QualitySettings.names.Length - 1);
            }
        }

        QualityText.text = QualitySettings.names[CurrentQuality];
    }

    public void AntiStropic(bool b)
    {
        if (b) { CurrentAS = (CurrentAS + 1) % 3; } else { if (CurrentAS != 0) { CurrentAS = (CurrentAS - 1) % 3; } else { CurrentAS = 2; } }

        switch (CurrentAS)
        {
            case 0:
                AntiStropicText.text = AnisotropicFiltering.Disable.ToString();
                break;
            case 1:
                AntiStropicText.text = AnisotropicFiltering.Enable.ToString();
                break;
            case 2:
                AntiStropicText.text = AnisotropicFiltering.ForceEnable.ToString();
                break;
        }
    }

    public void AntiAliasing(bool b)
    {
        CurrentAA = (b) ? (CurrentAA + 1) % 4 : (CurrentAA != 0) ? (CurrentAA - 1) % 4 : CurrentAA = 3;
        AntiAliasingText.text = AAOptions[CurrentAA];
    }

    public void VSyncCount(bool b)
    {
        CurrentVSC = (b) ? (CurrentVSC + 1) % 3 : (CurrentVSC != 0) ? (CurrentVSC - 1) % 3 : CurrentVSC = 2;
        vSyncText.text = VSCOptions[CurrentVSC];
    }

    public void blendWeights(bool b)
    {
        CurrentBW = (b) ? (CurrentBW + 1) % 3 : (CurrentBW != 0) ? (CurrentBW - 1) % 3 : CurrentBW = 2;
        switch (CurrentBW)
        {
            case 0:
                blendWeightsText.text = BlendWeights.OneBone.ToString();
                break;
            case 1:
                blendWeightsText.text = BlendWeights.TwoBones.ToString();
                break;
            case 2:
                blendWeightsText.text = BlendWeights.FourBones.ToString();
                break;
        }
    }

    public void Helmet(bool b)
    {
        CurrentHelmet = (CurrentHelmet + 1) % 2;

        switch (CurrentHelmet)
        {
            case 0:
                helmetText.text = "Off";
                break;
            case 1:
                helmetText.text = "On";
                break;
        }
    }

    public void Apply(bool save)
    {
        MainVolume = MainVolumeSlider.value;
        MusicVolume = MusicVolumeSlider.value;
        SoundVolume = SoundVolumeSlider.value;
        StaticApply(save);
    }

    public static void StaticApply(bool save)
    {
        if (save)
        {
            PlayerPrefs.SetInt("Quality", CurrentQuality);
            PlayerPrefs.SetInt("AnisoStropic", CurrentAS);
            PlayerPrefs.SetInt("AntiAliasing", CurrentAA);
            PlayerPrefs.SetInt("VSync", CurrentVSC);
            PlayerPrefs.SetInt("BlendWeight", CurrentBW);
            PlayerPrefs.SetInt("Helmet", CurrentHelmet);
            PlayerPrefs.SetFloat("MainVolume", AudioListener.volume);
            PlayerPrefs.SetFloat("MusicVolume", AudioListener.volume);
            PlayerPrefs.SetFloat("SoundVolume", AudioListener.volume);
        }

        QualitySettings.SetQualityLevel(CurrentQuality);

        switch (CurrentAS)
        {
            case 0:
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                break;
            case 1:
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                break;
            case 2:
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                break;
        }

        switch (CurrentAA)
        {
            case 0:
                QualitySettings.antiAliasing = 0;
                break;
            case 1:
                QualitySettings.antiAliasing = 2;
                break;
            case 2:
                QualitySettings.antiAliasing = 4;
                break;
            case 3:
                QualitySettings.antiAliasing = 8;
                break;
        }

        switch (CurrentVSC)
        {
            case 0:
                QualitySettings.vSyncCount = 0;
                break;
            case 1:
                QualitySettings.vSyncCount = 1;
                break;
            case 2:
                QualitySettings.vSyncCount = 2;
                break;
        }

        switch (CurrentBW)
        {
            case 0:
                QualitySettings.blendWeights = BlendWeights.OneBone;
                break;
            case 1:
                QualitySettings.blendWeights = BlendWeights.TwoBones;
                break;
            case 2:
                QualitySettings.blendWeights = BlendWeights.FourBones;
                break;
        }

        EyetrackerUIController.Instance().HelmetSet(CurrentHelmet == 1);

        AudioListener.volume = MainVolume;
        //music set
        //sound set
    }

    void GetInfoOptions()
    {
        QualityText.text = QualitySettings.names[CurrentQuality];

        switch (CurrentAS)
        {
            case 0:
                AntiStropicText.text = AnisotropicFiltering.Disable.ToString();
                break;
            case 1:
                AntiStropicText.text = AnisotropicFiltering.Enable.ToString();
                break;
            case 2:
                AntiStropicText.text = AnisotropicFiltering.ForceEnable.ToString();
                break;
        }

        AntiAliasingText.text = AAOptions[CurrentAS];
        vSyncText.text = VSCOptions[CurrentVSC];

        switch (CurrentBW)
        {
            case 0:
                blendWeightsText.text = BlendWeights.OneBone.ToString();
                break;
            case 1:
                blendWeightsText.text = BlendWeights.TwoBones.ToString();
                break;
            case 2:
                blendWeightsText.text = BlendWeights.FourBones.ToString();
                break;
        }

        helmetText.text = CurrentHelmet == 1 ? "On" : "Off";
        MainVolumeSlider.value = MainVolume;
        MusicVolumeSlider.value = MusicVolume;
        SoundVolumeSlider.value = SoundVolume;
    }
}
