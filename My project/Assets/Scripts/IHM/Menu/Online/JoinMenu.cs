using UnityEngine;
using TMPro;
using System.Threading;
using UnityEngine.UI;

namespace blockade.Blockade_IHM
{
    public class JoinMenu : MonoBehaviour
    {
        public string JoinerCode;

        public string points;
        public int i;
        public bool wait;
        public TMP_Text PointText;

        public GameObject menuObject;
        public GameObject waitScreenObject;
        public TMP_Text PlayerNameText;

        public Button JoinButton;

        [SerializeField] private GameObject game;
        [SerializeField] private GameObject menu;
        [SerializeField] private GameObject waitScreen;
        [SerializeField] private GameObject chat;

        public GameObject ErrorPopupObj;
        public TMP_Text ErrorPopup;

        /// <summary>
        /// Par Martin GADET
        /// Méthode Start qui set les valeur de départ pour l'écran d'attente
        /// Publique
        /// </summary>
        /// <returns></returns>
        void Start()
        {
            points = "";
            i = 0;
            wait = false;
            DisplayPlayerName(ConnectionMenu.PlayerName);
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui se lance toute les secondes pour afficher l'écran d'attente
        /// Publique
        /// </summary>
        /// <returns></returns>
        void Update()
        {
            // TODO récupérer l'info de lancement de partie
            // si partie lancée alors appeler
            // UIManager.SetTypePartie("ONLINE");
            // UIManager.PlayGame();
            // sinon
            if (wait == true)
            {
                points += ".";
                PointText.SetText(points);
                i += 1;
                if (i == 3)
                {
                    points = "";
                    i = 0;
                }
                Thread.Sleep(500);
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
        /// Méthode qui set le code rentré par le joueur qui rejoint
        /// Publique
        /// </summary>
        /// <returns></returns>
        public void SetJoinerCode(string inputCode)
        {
            ErrorPopupObj.SetActive(false);
            JoinerCode = UIManager.getGameCode(inputCode);
            Debug.Log("JoinerCode : " + JoinerCode);
        }

        /// <summary>
        /// Par Martin GADET
        /// 
        /// Appelle la fonction de rejoindre une partie
        /// 
        /// Publique
        /// </summary>
        public void ClickButton()
        {
            if (!UIManager.CheckCode(JoinerCode))
            {
                ErrorPopupObj.SetActive(true);
                ErrorPopup.SetText("Le code doit être un nombre à 4 chiffres.");
            }
            // TODO si le code est incorrect
            // ErrorPopupObj.SetActive(true);
            // ErrorPopup.SetText("Le code est incorrect.");
            else
            {
                ErrorPopupObj.SetActive(false);
                wait = true;
                Debug.Log("Joiner player " + ConnectionMenu.PlayerName);
                // TODO : game.GetComponent<Online>().fonction(PlayerName);

                this.menu.SetActive(false);
                this.waitScreen.SetActive(true);
                this.chat.SetActive(false);
            }
        }
    }
}