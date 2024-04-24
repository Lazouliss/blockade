using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace blockade.Blockade_IHM
{
    public class Chat : MonoBehaviour
    {
        public TMP_Text ChatContent;

        /// <summary>
        /// Par Martin GADET
        /// Méthode Start lancant le test
        /// Publique
        /// </summary>
        /// <returns></returns>
        // void Start()
        // {
        //     StartCoroutine(test());
        // }

        /// <summary>
        /// Par Martin GADET
        /// Méthode test pour test d'affichage
        /// Publique
        /// </summary>
        /// <returns></returns>
        // IEnumerator test()
        // {
        //     while (true)
        //     {
        //         addMessage("marty", "blablabla");
        //         yield return new WaitForSeconds(5f);
        //     }
        // }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui crée le message a afficher
        /// Publique
        /// </summary>
        /// <returns>message</returns>
        public string createMessageToSend(string PlayerName, string message)
        {
            string textToSend = PlayerName + " > " + message + "\n";
            return textToSend;
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui affiche le message
        /// Publique
        /// </summary>
        /// <returns></returns>
        public void addMessage(string playerName, string message)
        {
            ChatContent.text += createMessageToSend(playerName, message);
        }
    }
}