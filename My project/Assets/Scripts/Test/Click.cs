using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using blockade.Blockade_common;
using sys = System;

public class Click : MonoBehaviour
{
    public IHM ihm;
    public void ClickRandomAction()
    {
        sys.Random rand = new sys.Random();
        switch (rand.Next(3))
        {
            case 0:
                Common.DTOPawn tempPawn = new Common.DTOPawn();
                tempPawn.startPos = (0, 0);
                tempPawn.destPos = (1, 1);
                tempPawn.mooves = (Common.Direction.UP, Common.Direction.RIGHT);
                ihm.sendDTO(tempPawn);
                break;

            case 1:
                Common.DTOWall wall = new Common.DTOWall();
                wall.coord1 = (0, 0);
                wall.coord2 = (0, 1);
                wall.direction = Common.Direction.UP;
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
}
