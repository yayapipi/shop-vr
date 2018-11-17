using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour {
    public Text QualityText = null;
    private int CurrentQuality = 0;

    public Text AntiStropicText = null;
    private int CurrentAS = 0;

    public Text AntiAliasingText = null;
    private int CurrentAA = 0;
    private string[] AAOptions = new string[] { "X0", "X2", "X4", "X8" };

    public Text vSyncText = null;
    private int CurrentVSC = 0;
    private string[] VSCOptions = new string[] { "Don't Sync", "Every V Blank", "Every Second V Blank" };

    public Text blendWeightsText = null;
    private int CurrentBW = 0;

    public Text helmetText = null;
    private int CurrentHelmet = 0;

    public Slider MainVolumeSlider = null;
    public Slider MusicVolumeSlider = null;
    public Slider SoundVolumeSlider = null;

    void Awake()
    {
        GetInfoOptions();
    }

	// Use this for initialization
	void Start () 
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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

    public void Apply()
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

        //helmet

        AudioListener.volume = MainVolumeSlider.value;
        //music
        //sound
    }

    void GetInfoOptions()
    {
        if (PlayerPrefs.HasKey("MainVolume")) { MainVolumeSlider.value = PlayerPrefs.GetInt("MainVolume"); } else { MainVolumeSlider.value = AudioListener.volume; }
        if (PlayerPrefs.HasKey("MusicVolume")) { MusicVolumeSlider.value = PlayerPrefs.GetInt("MusicVolume"); } else { MusicVolumeSlider.value = 1; }
        if (PlayerPrefs.HasKey("SoundVolume")) { SoundVolumeSlider.value = PlayerPrefs.GetInt("SoundVolume"); } else { SoundVolumeSlider.value = 1; }

        QualityText.text = QualitySettings.names[CurrentQuality];

        switch (QualitySettings.anisotropicFiltering)
        {
            case AnisotropicFiltering.Disable:
                AntiStropicText.text = AnisotropicFiltering.Disable.ToString();
                CurrentAS = 0;
                break;
            case AnisotropicFiltering.Enable:
                AntiStropicText.text = AnisotropicFiltering.Enable.ToString();
                CurrentAS = 1;
                break;
            case AnisotropicFiltering.ForceEnable:
                AntiStropicText.text = AnisotropicFiltering.ForceEnable.ToString();
                CurrentAS = 2;
                break;
        }

        switch (QualitySettings.antiAliasing)
        {
            case 0:
                AntiAliasingText.text = AAOptions[0];
                CurrentAS = 0;
                break;
            case 2:
                AntiAliasingText.text = AAOptions[1];
                CurrentAS = 1;
                break;
            case 4:
                AntiAliasingText.text = AAOptions[2];
                CurrentAS = 2;
                break;
            case 8:
                AntiAliasingText.text = AAOptions[3];
                CurrentAS = 3;
                break;
        }

        vSyncText.text = VSCOptions[QualitySettings.vSyncCount];
        CurrentVSC = QualitySettings.vSyncCount;

        switch (QualitySettings.blendWeights)
        {
            case BlendWeights.OneBone:
                blendWeightsText.text = BlendWeights.OneBone.ToString();
                CurrentBW = 0;
                break;
            case BlendWeights.TwoBones:
                blendWeightsText.text = BlendWeights.TwoBones.ToString();
                CurrentBW = 1;
                break;
            case BlendWeights.FourBones:
                blendWeightsText.text = BlendWeights.FourBones.ToString();
                CurrentBW = 2;
                break;
        }
    }
}
