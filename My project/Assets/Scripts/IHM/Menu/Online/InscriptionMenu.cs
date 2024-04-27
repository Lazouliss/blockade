using System;
using UnityEngine;

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
        public void SetPlayerName (string inputName)
        {   
            if(UIManager.SetPlayerName(inputName) != "0")
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
        public void SetPlayerPassword1 (string inputPassword)
        {   
            if(UIManager.SetPlayerPassword(inputPassword) != "0")
            {
                PlayerPassword1 = UIManager.SetPlayerPassword(inputPassword);
                // Debug.Log("Password1 : " + PlayerPassword1);
            }
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui set le mot de passe du compte du joueur
        /// Publique
        /// </summary>
        /// <returns></returns>
        public void SetPlayerPassword2 (string inputPassword)
        {   
            if(UIManager.SetPlayerPassword(inputPassword) != "0")
            {
                PlayerPassword2 = UIManager.SetPlayerPassword(inputPassword);
                // Debug.Log("Password2 : " + PlayerPassword2);
            }
            try
            {
                if(!CheckEqualityPassword(PlayerPassword1, PlayerPassword2))
                {
                    throw new Exception("Les mots de passe sont différents.");
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning("Erreur : " + e.Message);
            }
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
    }
}