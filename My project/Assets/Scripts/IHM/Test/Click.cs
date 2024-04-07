using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using blockade.Blockade_common;
using sys = System;
using System.Linq;

/// <summary>
/// Par Thomas MONTIGNY
/// 
/// Fonction de clique de test, pour jouer un DTO aléatoire et tester la réception des DTOs
/// </summary>
public class Click : MonoBehaviour
{
    public IHM ihm;
    private sys.Random rand;

    // Start is called before the first frame update
    void Start()
    {
        rand = new sys.Random();
    }

    /// <summary>
    /// Par Thomas MONTIGNY
    /// 
    /// A destination de l'IHM et seulement pour les tests
    /// Crée un DTO aléatoire : pion, mur ou erreur
    /// Publique
    /// </summary>
    public void ClickRandomAction()
    {
        switch (rand.Next(3))
        {
            case 0:
                // send pawn and gamestate
                ihm.sendDTO(getRandomDTOPawn());
                ihm.sendDTO(getRandomDTOGameState());
                break;

            case 1:
                // send wall and gamestate
                ihm.sendDTO(getRandomDTOWall());
                ihm.sendDTO(getRandomDTOGameState());
                break;

            case 2:
                Common.DTOError error = new Common.DTOError();
                error.errorCode = rand.Next(10);
                ihm.sendDTO(error);
                break;

            default: Debug.Log("case default !"); break;
        }
    }

    /// <summary>
    /// Par Thomas MONTIGNY
    /// 
    /// Retourne une direction aléatoire
    /// Privée
    /// </summary>
    /// <returns></returns>
    private Common.Direction getRandomDirection()
    {
        switch (rand.Next(4))
        {
            case 0: return Common.Direction.UP;
            case 1: return Common.Direction.DOWN;
            case 2: return Common.Direction.LEFT;
            case 3: return Common.Direction.RIGHT;
            default: Debug.Log("case default during getRandomDirection"); return Common.Direction.UP;
        }
    }

    /// <summary>
    /// Par Thomas MONTIGNY
    /// 
    /// Génère un DTO pion aléatoire
    /// Privée
    /// </summary>
    /// <returns></returns>
    private Common.DTOPawn getRandomDTOPawn()
    {
        Common.DTOPawn tempPawn = new Common.DTOPawn();
        tempPawn.startPos = (0, 0);
        tempPawn.destPos = (0, 0);
        // create the list of mooves
        tempPawn.mooves = new List<Common.Direction>();
        tempPawn.mooves.Add(getRandomDirection());
        tempPawn.mooves.Add(getRandomDirection());

        return tempPawn;
    }

    /// <summary>
    /// Par Thomas MONTIGNY
    /// 
    /// Génère un DTO mur aléatoire
    /// Privée
    /// </summary>
    /// <returns></returns>
    private Common.DTOWall getRandomDTOWall()
    {
        Common.DTOWall wall = new Common.DTOWall();
        wall.direction = getRandomDirection();
        int x = rand.Next(14);
        int y = rand.Next(11);
        int x2 = x;
        int y2 = y;
        // check to get coord2 next to coord1
        if (wall.direction == Common.Direction.UP || wall.direction == Common.Direction.DOWN)
        {
            x2 = x2 + 1;
        }
        else
        {
            y2 = y2 + 1;
        }
        wall.coord1 = ((uint)(int)x, (uint)(int)y);
        wall.coord2 = ((uint)(int)x2, (uint)(int)y2);
        wall.isAdd = true;

        return wall;
    }

    /// <summary>
    /// Par Thomas MONTIGNY
    /// 
    /// Génère un DTO état du jeu aléatoire
    /// Privée
    /// </summary>
    /// <returns></returns>
    private Common.DTOGameState getRandomDTOGameState()
    {
        Common.DTOGameState dtoGameState = new Common.DTOGameState();
        dtoGameState.yellowPlayer = new Common.DTOGameState.Player();
        // random number of walls left
        dtoGameState.yellowPlayer.verticalWalls = (uint)(int)rand.Next(9);
        dtoGameState.yellowPlayer.horizontalWalls = (uint)(int)rand.Next(9);
        dtoGameState.yellowPlayer.isPlaying = (rand.Next(2) == 0);      // generate random boolean

        dtoGameState.redPlayer = new Common.DTOGameState.Player();
        dtoGameState.redPlayer.verticalWalls = (uint)(int)rand.Next(9);
        dtoGameState.redPlayer.horizontalWalls = (uint)(int)rand.Next(9);
        dtoGameState.redPlayer.isPlaying = !(dtoGameState.yellowPlayer.isPlaying);      // invert yellowPlayer.isPlaying -> only one of the players must play

        return dtoGameState;
    }
}
