using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public float volumeValue;
    public bool animationValue;
    public string languageValue;

    public void setVolume (float volume)
    {
        audioMixer.SetFloat("volume", volume);
        volumeValue = volume;
        Debug.Log(volumeValue);
    }

    public void setAnimation (bool animation)
    {
        animationValue = animation;
        Debug.Log(animationValue);
    }

    public void setLanguage (int language)
    {
        switch(language)
        {
            case 0:
                languageValue = "Français";
                break;
            case 1:
                languageValue = "Anglais";
                break;
        }

        Debug.Log(languageValue);
    }
}
