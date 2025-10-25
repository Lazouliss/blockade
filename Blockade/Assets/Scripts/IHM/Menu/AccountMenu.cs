using UnityEngine;
using UnityEngine.UI;

namespace blockade.Blockade_IHM
{
    public class AccountMenu : MonoBehaviour
    {
        public GameObject chat;

        /// <summary>
        /// Par Martin GADET
        /// Méthode Start qui cache le chat textuel
        /// Publique
        /// </summary>
        /// <returns></returns>
        void Start()
        {
            this.chat.SetActive(false);
        }
    }
}