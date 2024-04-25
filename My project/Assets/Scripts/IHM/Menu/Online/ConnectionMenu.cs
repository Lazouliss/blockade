using UnityEngine;

namespace blockade.Blockade_IHM
{
    public class ConnectionMenu : MonoBehaviour
    {
        public static string PlayerName;
        public static string PlayerPassword;

        public GameObject ConnectionButton;

        /// <summary>
        /// Par Martin GADET
        /// Méthode Update vérifie si les variable PlayerName et PlayerPassword sont rempli
        /// Publique
        /// </summary>
        /// <returns></returns>
        void Update()
        {
            SetActiveButton();
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui set le mot de passe du compte du joueur
        /// Publique
        /// </summary>
        /// <returns></returns>
        public void SetActiveButton()
        {
            if (PlayerName != null && PlayerPassword != null)
            {
                ConnectionButton.SetActive(true);
            }
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui set le pseudo du compte du joueur
        /// Publique
        /// </summary>
        /// <returns></returns>
        public void SetPlayerName(string inputName)
        {
            if (UIManager.SetPlayerName(inputName) != "0")
            {
                PlayerName = UIManager.SetPlayerName(inputName);
                Debug.Log("Pseudo : " + PlayerName);
            }
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui set le mot de passe du compte du joueur
        /// Publique
        /// </summary>
        /// <returns></returns>
        public void SetPlayerPassword(string inputPassword)
        {
            if (UIManager.SetPlayerPassword(inputPassword) != "0")
            {
                PlayerPassword = UIManager.SetPlayerPassword(inputPassword);
                Debug.Log("Password : " + PlayerPassword);
            }
        }
    }
}