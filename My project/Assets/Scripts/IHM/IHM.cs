using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using blockade.Blockade_common;
using sys = System;

/// <summary>
/// Par Thomas MONTIGNY
/// 
/// Classe IHM global, s'occupe de l'envoi et de la rï¿½ception des DTOs
/// Gï¿½re la communication avec le pï¿½le algorithmie (logique de jeu)
/// </summary>
public class IHM : MonoBehaviour
{
    public Board board;                 // plateau

    // player id --> 1 or 2
    private int playerNumber;

    public GameObject cams;

    // Start is called before the first frame update
    void Start()
    {
        // DEBUG : numéro de joueur aléatoire pour que la caméra ne soit jamais sur le même joueur au départ
        /*
        sys.Random rand = new sys.Random();
        playerNumber = rand.Next(2)+1;
        */

        // Initialize the cam in function of player number
        if ( this.playerNumber == 1 )
        {
            Debug.Log("Setting cams for player 1");
            Vector3 newRotation = new Vector3(0.0f, 180.0f, 0.0f);
            cams.transform.eulerAngles = newRotation;
        }

        // DEBUG : to test the spin animation on game end
        // cams.GetComponent<Animator>().SetTrigger("trigger_spin");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Par Thomas MONTIGNY
    /// 
    /// Appelï¿½ par la logique de jeu
    /// Publique
    /// Fonction pour recevoir les DTOs
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
    /// Privï¿½e
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
    /// Privï¿½e
    /// </summary>
    /// <param name="dto"></param>
    private void applyDTOWall(Common.DTOWall dto)
    {
        Debug.Log("applyDTOWall, coord1 = " + dto.coord1 + ", coord2 = " + dto.coord2 + ", direction = " + dto.direction + ", isAdd = " + dto.isAdd);
        board.actionWall(dto);
    }

    /// <summary>
    /// Par Thomas MONTIGNY
    /// 
    /// Applique un DTO pion
    /// Privï¿½e
    /// </summary>
    /// <param name="dto"></param>
    private void applyDTOPawn(Common.DTOPawn dto)
    {
        Debug.Log("applyDTOPawn, startPos = " + dto.startPos + ", destPos = " + dto.destPos + ", mooves = " + dto.mooves[0] + ", " + dto.mooves[1]);
        board.moovePawn(dto);
    }

    /// <summary>
    /// Par Thomas MONTIGNY
    /// 
    /// Applique un DTO ï¿½tat du jeu
    /// Privï¿½e
    /// TODO : changer de joueur / envoyer les infos ï¿½ l'interface pour le nombre de murs restants ?
    /// </summary>
    /// <param name="dto"></param>
    private void applyDTOGameState(Common.DTOGameState dto)
    {
        if (dto.yellowPlayer.isPlaying)
        {
            Debug.Log("applyDTOGameState, yellowPlayer.verticalWalls = " + dto.yellowPlayer.verticalWalls + ", yellowPlayer.horizontalWalls = " + dto.yellowPlayer.horizontalWalls);
        }
        else
        {
            Debug.Log("applyDTOGameState, redPlayer.verticalWalls = " + dto.redPlayer.verticalWalls + ", redPlayer.horizontalWalls = " + dto.redPlayer.horizontalWalls);
        }

        //board.actionGameState(dto);
    }

    /// <summary>
    /// Par Thomas MONTIGNY
    /// 
    /// Applique un DTO erreur
    /// Privï¿½e
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
    /// Envoie un DTO ï¿½ la logique de jeu
    /// Publique
    /// </summary>
    /// <param name="dto"></param>
    public void sendDTOToLogic(object dto)
    {
        //logic.sendDTO(dto);
    }
}
