using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace blockade.Blockade_IHM
{
    public class AccountMenu : MonoBehaviour
    {
        public GameObject Chat;

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