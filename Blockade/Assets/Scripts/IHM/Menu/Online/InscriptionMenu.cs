using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace blockade.Blockade_IHM
{
    public class InscriptionMenu : MonoBehaviour
    {
        public static string PlayerName;
        public static string PlayerPassword1;
        public static string PlayerPassword2;

        public TMP_InputField inputPassword1;
        public TMP_InputField inputPassword2;

        public Button InscriptionButton;

        [SerializeField] private GameObject game;
        [SerializeField] private GameObject inscriptionMenu;
        [SerializeField] private GameObject connectionMenu;
        [SerializeField] private GameObject chat;

        public GameObject ErrorPopupObj;
        public TMP_Text ErrorPopup;

        void Start() 
        {
            inputPassword1.contentType = TMP_InputField.ContentType.Password;
            inputPassword2.contentType = TMP_InputField.ContentType.Password;
            InscriptionButton.interactable = false;
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
            if(PlayerName != null && PlayerPassword1 != null && PlayerPassword2 != null)
            {
                InscriptionButton.interactable = true;
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
        public void SetPlayerPassword1 (string inputPassword)
        {   
            ErrorPopupObj.SetActive(false);
            PlayerPassword1 = UIManager.getPlayerPassword(inputPassword);
            // Debug.Log("Password1 : " + PlayerPassword1);
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui set le mot de passe du compte du joueur
        /// Publique
        /// </summary>
        /// <returns></returns>
        public void SetPlayerPassword2 (string inputPassword)
        {   
            ErrorPopupObj.SetActive(false);
            PlayerPassword2 = UIManager.getPlayerPassword(inputPassword);
            // Debug.Log("Password2 : " + PlayerPassword2);
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui vérifie si les 2 mots de passe sont les mêmes
        /// Publique
        /// </summary>
        /// <returns>Boolean (true/false)</returns>
        public bool CheckEqualityPassword (string p1, string p2)
        {   
            if(p1 != p2)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Par Martin GADET
        /// 
        /// Appelle la fonction de création de compte du joueur
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
            else if (!UIManager.CheckPlayerPassword(PlayerPassword1))
            {
                ErrorPopupObj.SetActive(true);
                ErrorPopup.SetText("Le mot de passe doit contenir au moins 8 caracteres, une minuscule, une majuscule et un chiffre.");
            }
            else if (!UIManager.CheckPlayerPassword(PlayerPassword2))
            {
                ErrorPopupObj.SetActive(true);
                ErrorPopup.SetText("Le mot de passe doit contenir au moins 8 caracteres, une minuscule, une majuscule et un chiffre.");
            }
            else if (!CheckEqualityPassword(PlayerPassword1, PlayerPassword2))
            {
                ErrorPopupObj.SetActive(true);
                ErrorPopup.SetText("Les mots de passe sont différents.");
            }
            else
            {
                ErrorPopupObj.SetActive(false);
                Debug.Log("Sign out player " + PlayerName);
                // TODO : game.GetComponent<Online>().fonction(PlayerName, PlayerPassword);

                game.GetComponent<GameManager>().online.Register(PlayerName, PlayerPassword1,OnSignUpComplete);
            }
        }
        private void OnSignUpComplete(bool success)
        {
            if (success)
            {

                this.chat.SetActive(false);
                this.inscriptionMenu.SetActive(false);
                this.connectionMenu.SetActive(true);
            }
            else
            {
                ErrorPopupObj.SetActive(true);
                ErrorPopup.SetText("La création de compte a échoué.");
            }
        }
    }
}