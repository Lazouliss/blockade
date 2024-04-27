using System;
using UnityEngine;

namespace blockade.Blockade_IHM
{
    public class UIManager : MonoBehaviour
    {
        public static string typePartie; // ONLINE, JCJ, JCE, ECE

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui passe a la scène de jeu
        /// Publique
        /// </summary>
        /// <returns></returns>
        public static void PlayGame()
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Debug.Log("The game is starting");
            GameObject.Find("Game").GetComponent<IHM>().PlayGame(typePartie);
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui set le type de la partie
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
        /// Méthode pour vérifier si une chaîne est composée uniquement de caractères alphanumériques ou si elle a plus de 15 caractères
        /// Publique
        /// </summary>
        /// <returns>Boolean (true/false)</returns>
        public static bool CheckPlayerName(string str)
        {
            if (str.Length > 15)
            {
                return false;
            }
            foreach (char c in str)
            {
                if (!char.IsLetter(c) || char.IsWhiteSpace(c) || char.IsPunctuation(c) || char.IsSymbol(c))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui set le nom d'un joueur
        /// Publique
        /// </summary>
        /// <returns>inputName (nom rentré par le joueur)</returns>
        public static string SetPlayerName(string inputName)
        {
            try
            {
                if (!UIManager.CheckPlayerName(inputName))
                {
                    throw new Exception("Le nom du joueur ne doit pas contenir de caractères spéciaux et ne doit pas dépasser 15 caractères.");
                }
                return inputName;
            }
            catch (Exception e)
            {
                Debug.LogWarning("Erreur : " + e.Message);
                return "0";
            }
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode pour vérifier si une chaîne est composée d'au moins 8 caractère
        /// Publique
        /// </summary>
        /// <returns>Boolean (true/false)</returns>
        public static bool CheckPlayerPassword(string str)
        {
            if (str.Length < 8)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui set le mot de passe d'un joueur
        /// Publique
        /// </summary>
        /// <returns>inputPassword (nom rentré par le joueur)</returns>
        public static string SetPlayerPassword(string inputPassword)
        {
            try
            {
                if (!UIManager.CheckPlayerPassword(inputPassword))
                {
                    throw new Exception("Le mot de passe doit contenir 8 caractères minimum.");
                }
                return inputPassword;
            }
            catch (Exception e)
            {
                Debug.LogWarning("Erreur : " + e.Message);
                return "0";
            }
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui set le code rentré par un joueur
        /// Publique
        /// </summary>
        /// <returns>code (code rentré par le joueur)</returns>
        public static int SetCode(string inputCode)
        {
            int code = 0;

            try
            {
                code = Int32.Parse(inputCode);
                return code;
            }
            catch (FormatException)
            {
                Debug.Log("Put a integer number as code");
                return 0;
            }
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui set le niveau de l'IA
        /// Publique
        /// </summary>
        /// <returns>levelValue, levelString (numéro du niveau et string correspondante sélectionner par le joueur)</returns>
        public static (int, string) setIALevel(int level)
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