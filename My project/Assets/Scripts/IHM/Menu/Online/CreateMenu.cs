using UnityEngine;
using TMPro;

namespace blockade.Blockade_IHM
{
    public class CreateMenu : MonoBehaviour
    {
        public static string CreatorPlayerName;

        public string CreatorCode;

        public int nbPlayer;
        public TMP_Text CodeText;
        public TMP_Text NumberText;
        public TMP_Text PlayerNameText;

        [SerializeField] private GameObject game;
        [SerializeField] private GameObject createMenu;
        [SerializeField] private GameObject overlay;
        [SerializeField] private GameObject chat;
        [SerializeField] private UIManager ui;

        /// <summary>
        /// Par Martin GADET
        /// Méthode Start qui récupère le code de la partie et le nom du joueur et les affichent
        /// Publique
        /// </summary>
        /// <returns></returns>
        void Start()
        {
            string testcode = "1234"; // TODO : tu peux changer le code de la game ici
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
            DisplayNumberOfPlayer(1); // TODO : ici on regarde le nombre de joueur dans la game
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui set le nom du joueur qui crée la partie
        /// Publique
        /// </summary>
        /// <returns></returns>
        public void SetCreatorPlayerName(string inputName)
        {
            if (UIManager.getPlayerName(inputName) != "0")
            {
                CreatorPlayerName = UIManager.getPlayerName(inputName);
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
            CreatorCode = UIManager.getGameCode(inputCode);
            CodeText.SetText(CreatorCode);
            Debug.Log("CreatorCode : " + CreatorCode);
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
        }

        /// <summary>
        /// Par Martin GADET
        /// 
        /// Appelle la fonction de création de partie
        /// 
        /// Publique
        /// </summary>
        public void ClickButton()
        {
            Debug.Log("Creator player " + ConnectionMenu.PlayerName);
            // TODO : game.GetComponent<Online>().fonction(PlayerName);

            this.createMenu.SetActive(false);
            this.overlay.SetActive(true);
            this.chat.SetActive(true);
            UIManager.SetTypePartie("ONLINE");
            ui.PlayGame();
        }
    }
}