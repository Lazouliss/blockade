using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using blockade.Blockade_common;
using sys = System;

public class IHM : MonoBehaviour
{
    public Board board;                 // plateau

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // TODO --> recevoir une action du plateau / jouer une action ?
        /* 
        Common.DTOPawn dtoPawn = new Common.DTOPawn();
        Common.DTOGameState dtoGameState = new Common.DTOGameState();
        dtoGameState.yellowPlayer = new Common.DTOGameState.Player();
        dtoGameState.yellowPlayer.verticalWalls = 5; 
        dtoGameState.yellowPlayer.horizontalWalls = 4;
        dtoGameState.yellowPlayer.isPlaying = true;

        dtoGameState.redPlayer = new Common.DTOGameState.Player();
        dtoGameState.redPlayer.verticalWalls = 6; 
        dtoGameState.redPlayer.horizontalWalls = 6;
        dtoGameState.redPlayer.isPlaying = false; 
        */

        // Envoyer les nouveaux DTOs
        //transmitDTO(dto);
        //transmitDTO(dtoGameState);
    }

    /// <summary>
    /// Appelé par la logique de jeu
    /// Publique
    /// Fonction pour recevoir les DTOs
    /// </summary>
    /// <param name="dto"></param>
    public void sendDTO(object dto)
    {
        applyDTO(dto);
    }

    /// <summary>
    /// Pour appliquer un DTO
    /// Privée
    /// </summary>
    /// <param name="dto"></param>
    private void applyDTO(object dto)
    {
        switch(dto)
        {
            case Common.DTOWall dtoWall: applyDTOWall(dtoWall); break;

            case Common.DTOPawn dtoPawn: applyDTOPawn(dtoPawn); break;

            case Common.DTOError dtoError: applyDTOError(dtoError); break;

            default: Debug.Log("error during applying dto"); break;
        }
    }

    /// <summary>
    /// Applique un DTO mur
    /// Privée
    /// </summary>
    /// <param name="dto"></param>
    private void applyDTOWall(Common.DTOWall dto)
    {
        Debug.Log("applyDTOWall, coord1 = " + dto.coord1 + ", coord2 = " + dto.coord2 + ", direction = " + dto.direction + ", isAdd = " + dto.isAdd);
        board.actionWall(dto);
    }

    /// <summary>
    /// Applique un DTO pion
    /// Privée
    /// </summary>
    /// <param name="dto"></param>
    private void applyDTOPawn(Common.DTOPawn dto)
    {
        Debug.Log("applyDTOPawn, startPos = " + dto.startPos + ", destPos = " + dto.destPos + ", mooves = " + dto.mooves);
        board.moovePawn(dto);
    }

    /// <summary>
    /// Applique un DTO erreur
    /// Privée
    /// TODO : indiquer visuellement qu'il y a eu une erreur
    /// </summary>
    /// <param name="dto"></param>
    private void applyDTOError(Common.DTOError dto)
    {
        Debug.Log("applyDTOError, errorCode = " + dto.errorCode);
    }

    // TODO --> envoyer les DTOs

}
