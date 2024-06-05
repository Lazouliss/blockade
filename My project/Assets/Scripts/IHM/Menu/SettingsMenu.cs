using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace blockade.Blockade_IHM
{
    public class SettingsMenu : MonoBehaviour
    {
        public AudioMixer audioMixer;

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui set le volume de l'AudioMixer
        /// Publique
        /// </summary>
        /// <returns></returns>
        public void setVolume(float volume)
        {
            audioMixer.SetFloat("volume", volume);
        }
    }
}
