using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using blockade.Blockade_common;
using sys = System;

/// <summary>
/// Par Thomas MONTIGNY
/// 
/// Classe IHM global, s'occupe de l'envoi et de la r�ception des DTOs
/// G�re la communication avec le p�le algorithmie (logique de jeu)
/// </summary>
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
        
    }

    /// <summary>
    /// Par Thomas MONTIGNY
    /// 
    /// Appel� par la logique de jeu
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
    /// Priv�e
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
    /// Priv�e
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
    /// Priv�e
    /// </summary>
    /// <param name="dto"></param>
    private void applyDTOPawn(Common.DTOPawn dto)
    {
        Debug.Log("applyDTOPawn, startPos = " + dto.startPos + ", destPos = " + dto.destPos + ", mooves = " + dto.mooves);
        board.moovePawn(dto);
    }

    /// <summary>
    /// Par Thomas MONTIGNY
    /// 
    /// Applique un DTO �tat du jeu
    /// Priv�e
    /// TODO : changer de joueur / envoyer les infos � l'interface pour le nombre de murs restants ?
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
    /// Priv�e
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
    /// Envoie un DTO � la logique de jeu
    /// Publique
    /// </summary>
    /// <param name="dto"></param>
    public void sendDTOToLogic(object dto)
    {
        //logic.sendDTO(dto);
    }
}
