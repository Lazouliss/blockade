using UnityEngine;
using UnityEngine.Audio;

namespace blockade.Blockade_IHM
{
    public class SettingsMenu : MonoBehaviour
    {
        public AudioMixer audioMixer;
        public static float volumeValue;

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
    }
}