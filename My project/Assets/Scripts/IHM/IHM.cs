using UnityEngine;
using UnityEngine.UI;
using blockade.Blockade_common;
using sys = System;
using System.Threading;
using System.Reflection;
using System;

namespace blockade.Blockade_IHM
{
    /// <summary>
    /// Par Thomas MONTIGNY
    /// 
    /// Classe IHM global, s'occupe de l'envoi et de la reception des DTOs
    /// Gere la communication avec le pole algorithmie (logique de jeu)
    /// </summary>
    public class IHM : MonoBehaviour
    {
        public ApplyDTO gestionDTO;
        public Board board;
        private DTOLogic dtoLogic;

        public GameObject cams;
        private GameObject overlay;

        // Variables de partie
        private string typePartie;
        private const int BASE_NBWALLS = Common.MAX_WALLS;

        // Players informations
        private int current_player;
        private Player p1, p2;          // yellow, red
        
        // Only for GameEnd
        [SerializeField] private GameObject endGameMenu;
        [SerializeField] private GameObject ui;
        private int winner;

        // Structure d'un joueur
        public struct Player
        {
            public int verticalWalls, horizontalWalls;
            public bool isPlaying;
            public bool isPlacingWall;
        }

        // Start is called before the first frame update
        void Start()
        {
            // Init cams
            cams.SetActive(true);
        }

        // =================
        // Getters & Setters
        // =================

        /// <summary>
        /// Par Thomas MONTIGNY
        ///
        /// Renvoie les informations d'un joueur / d'une IA
        ///
        /// Publique
        /// </summary>
        /// <param name="id_player">entier : 1 ou 2</param>
        /// <returns>Player</returns>
        public Player GetPlayer(int id_player)
        {
            if (id_player == 1)
                return p1;
            else
                return p2;
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Renvoie l'id du joueur courant
        /// 
        /// Publique
        /// </summary>
        /// <returns></returns>
        public int GetCurrentPlayer()
        {
            return current_player;
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Change de joueur courant
        /// 
        /// Publique
        /// </summary>
        /// <param name="id_player"></param>
        public void SetCurrentPlayer(int id_player) { current_player = id_player; }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Change l'etat du placement de mur du joueur
        /// 
        /// Publique
        /// </summary>
        /// <param name="id_player"></param>
        /// <param name="new_state"></param>
        public void SetPlayerPlacingWall(int id_player, bool new_state)
        {
            if (id_player == 1)
            {
                p1.isPlacingWall = new_state;
            }
            else
            {
                p2.isPlacingWall = new_state;
            }
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Change le nombre de murs d'un joueur
        /// 
        /// Publique
        /// </summary>
        /// <param name="id_player"></param>
        /// <param name="verticalWalls"></param>
        /// <param name="horizontalWalls"></param>
        public void SetPlayerWalls(int id_player, int verticalWalls, int horizontalWalls)
        {
            if (id_player == 1)
            {
                p1.horizontalWalls = horizontalWalls;
                p1.verticalWalls = verticalWalls;
            }
            else
            {
                p2.horizontalWalls = horizontalWalls;
                p2.verticalWalls = verticalWalls;
            }
        }

        // Update is called once per frame
        /// <summary>
        /// Par Nolan Laroche
        /// 
        /// Getter qui permet de récuperer le type de parties
        /// </summary>
        public string GetTypePartie()
        {
            return typePartie;
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        ///
        /// Permet de changer la camera de cote (en fonction du numero de joueur)
        /// </summary>
        public void SwitchPlayerCamera()
        {
            Debug.Log("Rotating camera");
            cams.transform.Rotate(0, 180, 0, Space.Self);
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Met a jour le nombre de murs restants du joueur dans l'Overlay
        /// 
        /// Publique
        /// </summary>
        /// <param name="p">Player</param>
        public void UpdateRemainingWalls(Player p)
        {
            overlay.GetComponent<Overlay>().UpdateRemainingWalls("Vertical", p.verticalWalls);
            overlay.GetComponent<Overlay>().UpdateRemainingWalls("Horizontal", p.horizontalWalls);
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Appelle la fonction de changement d'action pour un joueur.
        /// </summary>
        public void SwitchActionPlayer()
        {
            overlay.GetComponent<Overlay>().SwitchActionPlayer();
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Appelle la fonction d'affichage de l'erreur.
        /// </summary>
        public void ToggleError(bool state)
        {
            overlay.GetComponent<Overlay>().ToggleError(state);
        }

        // ===============================
        // Logique de transmission des DTO
        // ===============================
        // Sert d'intermediaire entre la logique des dtos et le reste des scripts de l'IHM

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Fonction pour recevoir les DTOs, appele par la logique de jeu
        /// Publique
        /// </summary>
        /// <param name="dto"></param>
        public void sendDTO(object dto)
        {
            dtoLogic.sendDTO(dto);
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Envoie un DTO a la logique de jeu
        /// Publique
        /// </summary>
        /// <param name="dto"></param>
        public void sendDTOToLogic(object dto)
        {
            dtoLogic.sendDTOToLogic(dto);
        }


        // ======================================
        // Logique de fonctionnement d'une partie
        // ======================================

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Lance la partie
        /// Publique
        /// </summary>
        public void PlayGame(string typePartie)
        {
            // Fully clean the board to create a new one
            ClearBoard();

            // Create DTOLogic
            dtoLogic = new DTOLogic(this, GetComponent<GameManager>());
            // Create ApplyDTO
            gestionDTO = GameObject.Find("Board").GetComponent<ApplyDTO>();
            gestionDTO.initApplyDTO(this);
            // Select type of game
            this.typePartie = typePartie;
            Debug.Log(typePartie);
            // Get the overlay
            overlay = GameObject.Find("Overlay");
            // Reset GameManager
            // TODO : dtoLogic.getGameLogic().ResetBoard();
            // Set camera rotation to 0
            cams.transform.eulerAngles = new Vector3(0, 0, 0);

            // Set timeScale to 1 (to be sure to move pawns correctly)
            Time.timeScale = 1;

            // Creation du plateau
            board.StartGame(BASE_NBWALLS);

            switch (typePartie)
            {
                case "ONLINE":
                    Debug.Log("Partie en ligne");
                    StartOnlineGame();
                    break;

                case "JCJ":
                    Debug.Log("Partie JCJ");
                    StartJCJGame();
                    break;

                case "JCE":
                    Debug.Log("Partie JCE");
                    StartJCEGame();
                    break;

                case "ECE":
                    Debug.Log("Partie ECE");
                    StartECEGame();
                    break;

                default: Debug.Log("Something is broken..."); break;
            }
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Lance la fonction de nettoyage du plateau
        /// Privee
        /// </summary>
        private void ClearBoard()
        {
            board.ClearBoard();
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Initialise les variables pour une partie en ligne
        /// Privee
        /// </summary>
        private void StartOnlineGame()
        {
            // TODO
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Initialise les variables pour une partie JcJ hors ligne
        /// Privee
        /// </summary>
        private void StartJCJGame()
        {
            p1.verticalWalls = BASE_NBWALLS;
            p1.horizontalWalls = BASE_NBWALLS;
            p1.isPlaying = true;
            p1.isPlacingWall = false;

            UpdateRemainingWalls(p1);

            p2.verticalWalls = BASE_NBWALLS;
            p2.horizontalWalls = BASE_NBWALLS;
            p2.isPlaying = false;
            p2.isPlacingWall = false;

            UpdateRemainingWalls(p2);

            current_player = 1;
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Initialise les variables pour une partie JCE
        /// Privee
        /// </summary>
        private void StartJCEGame()
        {
            // TODO
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Initialise les variables pour une partie ECE
        /// Privee
        /// </summary>
        private void StartECEGame()
        {
            // TODO
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Fonction de fin de partie :
        /// - Desactive les inputs des joueurs sur le plateau
        /// - Fait le tour du plateau via une animation
        /// </summary>
        /// <param name="winner"></param>
        internal void endGame(uint winner)
        {
            // Set the winner
            this.winner = (int)winner;

            // Stop playing for all players
            p1.isPlaying = false;
            p2.isPlaying = false;

            // Set the camera to player camera if needed
            if (!overlay.GetComponent<Overlay>().GetPlayerCamState()) { overlay.GetComponent<Overlay>().SwitchCamera(); }

            // Spin the cam around the board
            cams.gameObject.GetComponent<Animator>().enabled = true;
            cams.GetComponent<Animator>().SetTrigger("trigger_spin");
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Affiche le menu de fin de partie, appele par la fonction qui s'occupe d'animer le mouvement de la camera en fin de partie
        /// </summary>
        internal void ShowEndGameMenu()
        {
            // And show the menu
            overlay.SetActive(false);
            endGameMenu.SetActive(true);
            ui.GetComponent<RawImage>().enabled = true;
            endGameMenu.GetComponent<EndMenu>().SelectWinner(current_player == this.winner);
        }
    }
}