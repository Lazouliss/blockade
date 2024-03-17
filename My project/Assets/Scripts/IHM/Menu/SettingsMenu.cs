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

    // Méthode qui set le volume dans une variable change celui du AudioMixer
    public void setVolume (float volume)
    {
        audioMixer.SetFloat("volume", volume);
        volumeValue = volume;
        Debug.Log(volumeValue);
        // TODO trouver une musique
    }

    // Méthode qui set le booléen des animations
    public void setAnimation (bool animation)
    {
        animationValue = animation;
        Debug.Log(animationValue);
    }

    // Méthode qui set le langage des menus
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
        // TODO chnager le langage quand nécessaire
    }
}
