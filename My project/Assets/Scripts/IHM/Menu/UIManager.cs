using System;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace blockade.Blockade_IHM
{
    public class UIManager : MonoBehaviour
    {
        public static string typePartie; // ONLINE, JCJ, JCE, ECE

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui affiche le plateau
        /// Publique
        /// </summary>
        /// <returns></returns>
        public void PlayGame()
        {
            Debug.Log("The game is starting");
            RawImage background = GetComponent<RawImage>();
            background.enabled = false;
            GameObject.Find("Game").GetComponent<IHM>().PlayGame(typePartie);
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui set le type de la partie (ONLINE, JCJ, JCE, ECE)
        /// Publique
        /// </summary>
        /// <returns></returns>
        public static void SetTypePartie(string type)
        {
            typePartie = type;
            Debug.Log(typePartie);
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode pour vérifier si le nom du joueur est composé uniquement de caractères alphanumériques ou si elle a plus de 12 caractères
        /// Publique
        /// </summary>
        /// <returns>Boolean (true/false)</returns>
        public static bool CheckPlayerName(string login)
        {
            if (login.Length > 12)
                return false;

            if (!Regex.IsMatch(login, @"^[a-zA-Z0-9]+$"))
                return false;

            return true;
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui récupère le nom d'un joueur
        /// Publique
        /// </summary>
        /// <returns>inputName (nom rentré par le joueur)</returns>
        public static string getPlayerName(string inputName)
        {
            if (!UIManager.CheckPlayerName(inputName))
            {
                // TODO afficher popup
                return "0";
            }
            return inputName;
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode pour vérifier si le mdp est composé d'au moins 8 caractère
        /// Publique
        /// </summary>
        /// <returns>Boolean (true/false)</returns>
        public static bool CheckPlayerPassword(string password)
        {
            if (password.Length < 8)
                return false;

            bool hasUpperCase = false;
            bool hasLowerCase = false;
            bool hasDigit = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c))
                    hasUpperCase = true;
                else if (char.IsLower(c))
                    hasLowerCase = true;
                else if (char.IsDigit(c))
                    hasDigit = true;
            }

            if (hasUpperCase && hasLowerCase && hasDigit)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui récupère le mot de passe d'un joueur
        /// Publique
        /// </summary>
        /// <returns>inputPassword (nom rentré par le joueur)</returns>
        public static string getPlayerPassword(string inputPassword)
        {
            if (!UIManager.CheckPlayerPassword(inputPassword))
            {
                // TODO afficher pop up
                return "0";
            }
            return inputPassword;
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui récupère le code de partie rentré par un joueur
        /// Publique
        /// </summary>
        /// <returns>code (code rentré par le joueur)</returns>
        public static bool CheckCode(string code)
        {
            int parsedCode;
            if (!int.TryParse(code, out parsedCode))
                return false;

            if (code.Length != 4)
                return false;

            return true;
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui récupère le mot de passe d'un joueur
        /// Publique
        /// </summary>
        /// <returns>inputPassword (nom rentré par le joueur)</returns>
        public static int getGameCode(string inputCode)
        {
            if (!UIManager.CheckCode(inputCode))
            {
                // TODO afficher pop up
                return 0;
            }
            return int.Parse(inputCode);
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui récupère le niveau de l'IA
        /// Publique
        /// </summary>
        /// <returns>levelValue, levelString (numéro du niveau et string correspondante sélectionner par le joueur)</returns>
        public static (int, string) getIALevel(int level)
        {
            int levelValue = 1;
            string levelString = "Facile";
            switch (level)
            {
                case 0:
                    levelValue = 1;
                    levelString = "Facile";
                    break;
                case 1:
                    levelValue = 2;
                    levelString = "Moyen";
                    break;
                case 2:
                    levelValue = 3;
                    levelString = "Difficile";
                    break;
            }

            return (levelValue, levelString);
        }
    }
}