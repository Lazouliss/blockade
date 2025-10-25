using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace blockade.Blockade_IHM
{
    public class ConnectionMenu : MonoBehaviour
    {
        public static string PlayerName;
        public static string PlayerPassword;

        public TMP_InputField inputPassword;
        
        public Button ConnectionButton;

        [SerializeField] private GameObject game;
        [SerializeField] private GameObject connectionMenu;
        [SerializeField] private GameObject mainMenu;
        [SerializeField] private GameObject chat;

        public GameObject ErrorPopupObj;
        public TMP_Text ErrorPopup;

        void Start() 
        {
            inputPassword.contentType = TMP_InputField.ContentType.Password;
            ConnectionButton.interactable = false;
        }

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
                ConnectionButton.interactable = true;
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
            ErrorPopupObj.SetActive(false);
            PlayerName = UIManager.getPlayerName(inputName);
            Debug.Log("Pseudo : " + PlayerName);
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui set le mot de passe du compte du joueur
        /// Publique
        /// </summary>
        /// <returns></returns>
        public void SetPlayerPassword(string inputPassword)
        {
            ErrorPopupObj.SetActive(false);
            PlayerPassword = UIManager.getPlayerPassword(inputPassword);
            // Debug.Log("Password : " + PlayerPassword);
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Appelle la fonction de connexion du joueur
        /// 
        /// Publique
        /// </summary>
        public void ClickButton()
        {
            if (!UIManager.CheckPlayerName(PlayerName))
            {
                ErrorPopupObj.SetActive(true);
                ErrorPopup.SetText("Le nom de joueur doit contenir moins de 12 caracteres et ne pas contenir de caractere special.");
            }
            else if (!UIManager.CheckPlayerPassword(PlayerPassword))
            {
                ErrorPopupObj.SetActive(true);
                ErrorPopup.SetText("Le mot de passe doit contenir au moins 8 caracteres, une minuscule, une majuscule et un chiffre.");
            }
            else
            {
                ErrorPopupObj.SetActive(false);
                Debug.Log("Connecting player "+PlayerName);
                // TODO : game.GetComponent<Online>().fonction(PlayerName, PlayerPassword);
                // si mot de passe incorrect
                // ErrorPopupGO.SetActive(true);
                // ErrorPopup.SetText("Le mot de passe est incorrect.");
                game.GetComponent<GameManager>().online.Login(PlayerName, PlayerPassword,OnLoginComplete);
            }
        }
        private void OnLoginComplete(bool success, string jwt)
        {
            if (success)
            {
                Debug.Log("Login successful, JWT: " + jwt);

                this.connectionMenu.SetActive(false);
                this.mainMenu.SetActive(true);
                this.chat.SetActive(true);
            }
            else
            {
                ErrorPopupObj.SetActive(true);
                ErrorPopup.SetText("Le mot de passe est incorrect.");
            }
        }
    }
}