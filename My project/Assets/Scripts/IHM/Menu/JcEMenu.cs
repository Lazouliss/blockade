using UnityEngine;
using TMPro;

namespace blockade.Blockade_IHM
{
    public class JcEMenu : MonoBehaviour
    {
        public static string PlayerName;
        public static int levelValue;
        public static string levelString;

        public TMP_Text PlayerNameText;

        /// <summary>
        /// Par Martin GADET
        /// Méthode Start qui récupère le nom du joueur et l'affiche
        /// Publique
        /// </summary>
        /// <returns></returns>
        void Start()
        {
            DisplayPlayerName(ConnectionMenu.PlayerName);
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
        /// Méthode qui set le niveau de l'IA adverse
        /// Publique
        /// </summary>
        /// <returns></returns>
        public void setIALevel(int level)
        {
            levelValue = UIManager.setIALevel(level).Item1;
            levelString = UIManager.setIALevel(level).Item2;
            Debug.Log("levelValue : " + levelValue);
            Debug.Log("levelString : " + levelString);
        }
    }
}