using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace blockade.Blockade_IHM
{
    public class SettingsMenu : MonoBehaviour
    {
        public AudioMixer audioMixer;
        public static float volumeValue;
        public static bool animationValue;
        public static string languageValue;

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui set le volume dans une variable change celui du AudioMixer
        /// Publique
        /// </summary>
        /// <returns></returns>
        public void setVolume(float volume)
        {
            audioMixer.SetFloat("volume", volume);
            volumeValue = volume;
            Debug.Log(volumeValue);
            // TODO trouver une musique
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui set le booléen des animations
        /// Publique
        /// </summary>
        /// <returns></returns>
        public void setAnimation(bool animation)
        {
            animationValue = animation;
            Debug.Log(animationValue);
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui set le langage des menus
        /// Publique
        /// </summary>
        /// <returns></returns>
        public void setLanguage(int language)
        {
            switch (language)
            {
                case 0:
                    languageValue = "Français";
                    break;
                case 1:
                    languageValue = "Anglais";
                    break;
            }

            Debug.Log(languageValue);
            // TODO changer le langage quand nécessaire
        }
    }
}