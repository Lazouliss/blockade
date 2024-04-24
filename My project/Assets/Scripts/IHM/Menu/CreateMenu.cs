using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace blockade.Blockade_IHM
{
    public class CreateMenu : MonoBehaviour
    {
        public static string CreatorPlayerName;

        public int CreatorCode;

        public int nbPlayer;
        public TMP_Text CodeText;
        public TMP_Text NumberText;
        public TMP_Text PlayerNameText;

        /// <summary>
        /// Par Martin GADET
        /// Méthode Start qui récupère le code de la partie et le nom du joueur et les affichent
        /// Publique
        /// </summary>
        /// <returns></returns>
        void Start()
        {
            string testcode = "1234";
            DisplayCreatorCode(testcode);
            DisplayPlayerName(ConnectionMenu.PlayerName);
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode Update qui récupère le nombre de joueur chaque seconde et l'affiche
        /// Publique
        /// </summary>
        /// <returns></returns>
        void Update()
        {
            DisplayNumberOfPlayer(1);
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui set le nom du joueur qui crée la partie
        /// Publique
        /// </summary>
        /// <returns></returns>
        public void SetCreatorPlayerName(string inputName)
        {
            if (UIManager.SetPlayerName(inputName) != "0")
            {
                CreatorPlayerName = UIManager.SetPlayerName(inputName);
                Debug.Log("CreatorPLayer : " + CreatorPlayerName);
            }
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui affiche le nom du joueur
        /// Publique
        /// </summary>
        /// <returns></returns>
        public void DisplayPlayerName(string name)
        {
            PlayerNameText.SetText(name);
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui affiche le code de la partie
        /// Publique
        /// </summary>
        /// <returns></returns>
        public void DisplayCreatorCode(string inputCode)
        {
            if (UIManager.SetCode(inputCode) != 0)
            {
                CreatorCode = UIManager.SetCode(inputCode);
                CodeText.SetText(CreatorCode.ToString());
                Debug.Log("CreatorCode : " + CreatorCode);
            }
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui affiche le nombre de joueur dans la partie
        /// Publique
        /// </summary>
        /// <returns></returns>
        public void DisplayNumberOfPlayer(int nb)
        {
            nbPlayer = nb;
            NumberText.SetText(nbPlayer.ToString());
            Debug.Log(nbPlayer);
            // TODO comment recup l'info ?
        }
    }
}