using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using blockade.Blockade_common;
using sys = System;

public class IHM : MonoBehaviour
{
    private List<object> receivedDTOs = new List<object>();               // la liste des DTOs en attente d'application
    private List<object> DTOsToSend = new List<object>();                 // la liste des DTOs à envoyer
    private object lastDTO = null;             // dernier DTO gardé pour pouvoir annuler un coup --> ne suffit pas si plusieurs erreurs d'affilé ?

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // for tests !!!!!!!!!!!!!!!!!!!
        randomAction();

        // Si pas de DTOs reçus, ne fait rien
        if (receivedDTOs.Count == 0) return;

        // Appliquer les DTOs
        foreach (var dto in receivedDTOs)
        {
            applyDTO(dto);
            lastDTO = dto;
        }

        // Effacer la liste des DTOs reçus
        receivedDTOs.Clear();

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

    // Fonction pour recevoir les DTOs
    public void receiveDTO(object dto)
    {
        receivedDTOs.Add(dto);
    }

    // Pour appliquer un DTO
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

    private void applyDTOWall(Common.DTOWall dto)
    {
        // TODO
        Debug.Log("applyDTOWall, coord1 = " + dto.coord1 + ", coord2 = " + dto.coord2 + ", direction = " + dto.direction + ", isAdd = " + dto.isAdd);
    }

    private void applyDTOPawn(Common.DTOPawn dto)
    {
        // TODO
        Debug.Log("applyDTOPawn, startPos = " + dto.startPos + ", destPos = " + dto.destPos + ", mooves = " + dto.mooves);
        // si mooves pas init, renvoie UP, UP (la première info trouvée dans Direction) -> à init avec null, null  
    }

    private void applyDTOError(Common.DTOError dto)
    {
        // TODO
        Debug.Log("applyDTOError, errorCode = " + dto.errorCode);
        // applyDTO(lastDTO);       ??
    }



    // TODO --> fonctions pour appliquer les DTOs --> dans la partie déplacement des pions ?

    // TODO --> envoyer les DTOs
    
    private void randomAction()
    {
        sys.Random rand = new sys.Random();
        switch (rand.Next(3))
        {
            case 0:
                Common.DTOPawn tempPawn = new Common.DTOPawn();
                tempPawn.startPos = (0, 0);
                tempPawn.destPos = (1, 1);
                tempPawn.mooves = (Common.Direction.UP, Common.Direction.RIGHT);
                receiveDTO(tempPawn);
                break;

            case 1:
                Common.DTOWall wall = new Common.DTOWall();
                wall.coord1 = (0, 0);
                wall.coord2 = (0, 1);
                wall.direction = Common.Direction.UP;
                receiveDTO(wall);
                break;

            case 2:
                Common.DTOError error = new Common.DTOError();
                error.errorCode = rand.Next(10);
                receiveDTO(error);
                break;

            default: Debug.Log("case default !"); break;
        }
    }
}
