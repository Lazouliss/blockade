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
            if (UIManager.getPlayerName(inputName) != "0")
            {
                PlayerName = UIManager.getPlayerName(inputName);
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
            if (UIManager.getPlayerPassword(inputPassword) != "0")
            {
                PlayerPassword = UIManager.getPlayerPassword(inputPassword);
                // Debug.Log("Password : " + PlayerPassword);
            }
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
            Debug.Log("Connecting player "+PlayerName);
            // TODO : game.GetComponent<Online>().fonction(PlayerName, PlayerPassword);

            this.connectionMenu.SetActive(false);
            this.mainMenu.SetActive(true);
            this.chat.SetActive(true);
        }
    }
}