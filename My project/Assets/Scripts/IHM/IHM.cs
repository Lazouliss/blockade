using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using blockade.Blockade_common;
using sys = System;
using Unity.VisualScripting;
using System;
using UnityEngine.UIElements;

/// <summary>
/// Par Thomas MONTIGNY
/// 
/// Classe IHM global, s'occupe de l'envoi et de la reception des DTOs
/// Gere la communication avec le pole algorithmie (logique de jeu)
/// </summary>
public class IHM : MonoBehaviour
{
    public Board board;                 // plateau

    // player id --> 1 or 2
    private int current_player;           // changer pour current_player

    public GameObject cams;
    public GameObject cams2;
    private GameObject overlay;

    private object last_DTO;

    // Variables de partie
    private string typePartie;
    private const int BASE_NBWALLS = Common.MAX_WALLS;
    private Player p1, p2;          // yellow, red

    // STOCKER LES JOUEURS COMME DANS LE COMMON.CS --> en "dto" et mettre � jour � chaque GameState re�u
    public struct Player
    {
        public int verticalWalls, horizontalWalls;
        public bool isPlaying;
        public bool isPlacingWall;              // TODO : autoriser seulement le déplacement des murs si vrai, sinon seulement le déplacement des pions
    }

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

    // Start is called before the first frame update
    void Start()
    {
        // DEBUG : num�ro de joueur al�atoire pour que la cam�ra ne soit jamais sur le m�me joueur au d�part
        /*
        sys.Random rand = new sys.Random();
        current_player = rand.Next(2)+1;
        */

        // R�cup�ration de l'overlay
        overlay = GameObject.Find("Overlay");
        /*
        // For tests
        PlayGame("JCJ");

        // Tests
        current_player = 1;
        p1.isPlacingWall = true;

        // Initialize the cam in function of player number
        SwitchPlayerCamera(current_player);
        */
        // DEBUG : to test the spin animation on game end
        //cams.GetComponent<Animator>().SetTrigger("trigger_spin");
    }

    /// <summary>
    /// Par Thomas MONTIGNY
    ///
    /// Permet de changer la camera de cote (en fonction du numero de joueur)
    /// </summary>
    /// <param name="current_player"></param>
    private void SwitchPlayerCamera(int current_player)
    {
        if (current_player == 2)
        {
            cams.SetActive(false);
            cams2.SetActive(true);
        }
        else
        {
            cams.SetActive(true);
            cams2.SetActive(false);
        }

        // NOT WORKING ANYMORE
        /*
        if (current_player == 2)
        {
            Debug.Log("Setting cams for player 2 "+ cams.transform.eulerAngles);
            Vector3 newRotation = new Vector3(0.0f, 180.0f, 0.0f);
            cams.transform.eulerAngles = newRotation;
            //cams.transform.Rotate(0, 180, 0, Space.Self);
            Debug.Log("Setting cams for player 2 " + cams.transform.eulerAngles);
        } 
        else
        {
            Debug.Log("Setting cams for player 1");
            Vector3 newRotation = new Vector3(0.0f, 0.0f, 0.0f);
            cams.transform.eulerAngles = newRotation;
        }
        */
    }

    /// <summary>
    /// Par Thomas MONTIGNY
    /// 
    /// Lance la fonction de cr�ation du plateau
    /// Publique
    /// </summary>
    public void PlayGame(string typePartie)
    {
        this.typePartie = typePartie;
        overlay = GameObject.Find("Overlay");

        Debug.Log(typePartie);

        // Cr�ation du plateau
        board.GetComponent<Plateau>().StartGame(BASE_NBWALLS);

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

        SwitchPlayerCamera(current_player);
    }

    private void StartOnlineGame()
    {
        // TODO
    }

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

    private void StartJCEGame()
    {
        // TODO
    }

    private void StartECEGame()
    {
        // TODO
    }

    /// <summary>
    /// Par Thomas MONTIGNY
    /// 
    /// Met a jour le nombre de murs restants du joueur dans l'Overlay
    /// 
    /// Privee
    /// </summary>
    /// <param name="p">Player</param>
    private void UpdateRemainingWalls(Player p)
    {
        overlay.GetComponent<Overlay>().UpdateRemainingWalls("Vertical", p.verticalWalls);
        overlay.GetComponent<Overlay>().UpdateRemainingWalls("Horizontal", p.horizontalWalls);
    }

    // =============================================
    //      Fonctions DTOs (a changer de script)
    // =============================================

    /// <summary>
    /// Par Thomas MONTIGNY
    /// 
    /// Fonction pour recevoir les DTOs, appele par la logique de jeu
    /// Publique
    /// </summary>
    /// <param name="dto"></param>
    public void sendDTO(object dto)
    {
        applyDTO(dto);
    }

    /// <summary>
    /// Par Thomas MONTIGNY
    /// 
    /// Pour appliquer un DTO
    /// Privee
    /// </summary>
    /// <param name="dto"></param>
    private void applyDTO(object dto)
    {
        switch(dto)
        {
            case Common.DTOWall dtoWall: applyDTOWall(dtoWall); break;

            case Common.DTOPawn dtoPawn: applyDTOPawn(dtoPawn); break;

            case Common.DTOGameState dtoGameState: applyDTOGameState(dtoGameState); break;

            case Common.DTOError dtoError: applyDTOError(dtoError); break;

            default: Debug.Log("error during applying dto"); break;
        }
    }

    /// <summary>
    /// Par Thomas MONTIGNY
    /// 
    /// Applique un DTO mur
    /// Privee
    /// </summary>
    /// <param name="dto"></param>
    private void applyDTOWall(Common.DTOWall dto)
    {
        Debug.Log("applyDTOWall, coord1 = " + dto.coord1 + ", coord2 = " + dto.coord2 + ", direction = " + dto.direction + ", isAdd = " + dto.isAdd);
        board.actionWall(dto);

        // Le joueur viens de déplacer un mur donc sa prochaine action est de déplacer un pion
        if (current_player == 1)
        {
            p1.isPlacingWall = false;
        }
        else
        {
            p2.isPlacingWall = false;
        }
    }

    /// <summary>
    /// Par Thomas MONTIGNY
    /// 
    /// Applique un DTO pion
    /// Privee
    /// </summary>
    /// <param name="dto"></param>
    private void applyDTOPawn(Common.DTOPawn dto)
    {
        Debug.Log("applyDTOPawn, startPos = " + dto.startPos + ", destPos = " + dto.destPos + ", mooves = " + dto.mooves[0] + ", " + dto.mooves[1]);
        board.movePawn(dto);

        // Le joueur viens de déplacer un pion donc sa prochaine action est de déplacer un mur
        if (current_player == 1 && p1.verticalWalls > 0 && p1.horizontalWalls > 0)
        {
            p1.isPlacingWall = true;
        }
        else if (current_player == 2 && p2.verticalWalls > 0 && p2.horizontalWalls > 0)
        {
            p2.isPlacingWall = true;
        }
    }

    /// <summary>
    /// Par Thomas MONTIGNY
    /// 
    /// Applique un DTO etat du jeu
    /// Privee
    /// </summary>
    /// <param name="dto"></param>
    private void applyDTOGameState(Common.DTOGameState dto)
    {
        // Met le nombre de murs restants selon le joueur
        if (dto.yellowPlayer.isPlaying)
        {
            Debug.Log("applyDTOGameState, yellowPlayer.verticalWalls = " + dto.yellowPlayer.verticalWalls + ", yellowPlayer.horizontalWalls = " + dto.yellowPlayer.horizontalWalls);
            p1.horizontalWalls = (int)dto.yellowPlayer.horizontalWalls;
            p1.verticalWalls = (int)dto.yellowPlayer.verticalWalls;
        }
        else
        {
            Debug.Log("applyDTOGameState, redPlayer.verticalWalls = " + dto.redPlayer.verticalWalls + ", redPlayer.horizontalWalls = " + dto.redPlayer.horizontalWalls);
            p2.horizontalWalls = (int)dto.redPlayer.horizontalWalls;
            p2.verticalWalls = (int)dto.redPlayer.verticalWalls;
        }

        // Change current player and update Overlay
        if (dto.yellowPlayer.isPlaying && current_player == 2)
        {
            current_player = 1;
            UpdateRemainingWalls(p1);
        }
        else if (dto.redPlayer.isPlaying && current_player == 1)
        {
            current_player = 2;
            UpdateRemainingWalls(p2);
        }
        
        // Tourne la camera si besoin
        SwitchPlayerCamera(current_player);
    }

    /// <summary>
    /// Par Thomas MONTIGNY
    /// 
    /// Applique un DTO erreur
    /// Privee
    /// TODO : indiquer visuellement qu'il y a eu une erreur
    /// </summary>
    /// <param name="dto"></param>
    private void applyDTOError(Common.DTOError dto)
    {
        Debug.Log("applyDTOError, errorCode = " + dto.errorCode);
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
        //logic.sendDTO(dto);
    }
}
