using UnityEngine;
using UnityEngine.Audio;

namespace blockade.Blockade_IHM
{
    public class AccountMenu : MonoBehaviour
    {
        public GameObject Chat;
        public AudioMixer audioMixer;

        /// <summary>
        /// Par Martin GADET
        /// Méthode Start qui cache le chat textuel
        /// Publique
        /// </summary>
        /// <returns></returns>
        void Start()
        {
            Chat.SetActive(false);
        }
    }
}