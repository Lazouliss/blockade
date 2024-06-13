using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using blockade.Blockade_common;
using sys = System;
using blockade.Blockade_IHM;

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
    /// A destination de l'IHM et seulement pour les tests
    /// Cr�e un DTO al�atoire : pion, mur ou erreur
    /// </summary>
    public void ClickRandomAction()
    {
        switch (rand.Next(3))
        {
            case 0:
                Common.DTOPawn tempPawn = new Common.DTOPawn();
                tempPawn.startPos = (0, 0);
                tempPawn.destPos = (0, 0);var directions = new List<Common.Direction>
                {
                    getRandomDirection(),
                    getRandomDirection()
                };
                ihm.sendDTO(tempPawn);
                break;

            case 1:
                Common.DTOWall wall = new Common.DTOWall();
                wall.direction = getRandomDirection();
                int x = rand.Next(14);
                int y = rand.Next(11);
                int x2 = x;
                int y2 = y;
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
                ihm.sendDTO(wall);
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
    /// Retourne une direction al�atoire
    /// </summary>
    /// <returns></returns>
    private Common.Direction getRandomDirection()
    {
        switch(rand.Next(4)) 
        { 
            case 0: return Common.Direction.UP;
            case 1: return Common.Direction.DOWN;
            case 2: return Common.Direction.LEFT;
            case 3: return Common.Direction.RIGHT;
            default: Debug.Log("case default during getRandomDirection"); return Common.Direction.UP;
        }
    }
}
