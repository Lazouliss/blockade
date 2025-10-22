using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using blockade.Blockade_common;

public class DTOHandler
{
    private Common.DTO lastDTO = new Common.DTO();

    // Start is called before the first frame update
    void Start()
    {
        Common.DTOError test = new Common.DTOError() { errorCode = 0 };
        lastDTO.setNew(test);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public object simulateCreateDTO(string DTOType, (uint, uint)? posPawn = null, (uint, uint)? newPosPawn = null, (uint, uint)? firstPosWall = null, (uint, uint)? secondPosWall = null, Common.Direction? directionWall = null) {
        if (DTOType == "Pawn") {
            // Checking if the wall needed values are null
            if (posPawn == null || newPosPawn == null) {
                throw new ArgumentException($"posPawn and newPosPawn cannot be null or empty to create a Pawn DTO");
            } else {
                // Creating non optional values to pass in the DTO creation
                (uint, uint) pos = ((uint, uint)) posPawn;
                (uint, uint) newPos = ((uint, uint)) newPosPawn;

                Common.DTOPawn Pawn = new Common.DTOPawn() { startPos = pos, destPos = newPos, mooves = null };
                Debug.Log("Pawn DTO created " + Pawn.startPos + " " + Pawn.destPos);
                return Pawn;
            }
        } 
        else {
            // Checking if the wall needed values are null
            if (firstPosWall == null || secondPosWall == null || directionWall == null) {
                throw new ArgumentException($"firstPosWall, secondPosWall and directionWall cannot be null or empty to create a Wall DTO");
            } else {
                // Creating non optional values to pass in the DTO creation
                (uint, uint) firstPos = ((uint, uint)) firstPosWall;
                (uint, uint) secondPos = ((uint, uint)) secondPosWall;
                Common.Direction dir = (Common.Direction) directionWall;

                Common.DTOWall Wall = new Common.DTOWall() { coord1 = firstPos, coord2 = secondPos, direction = dir, isAdd = null };
                Debug.Log("Wall DTO created " + Wall.coord1 + " " + Wall.coord2 + " " + Wall.direction);
                return Wall;
            }
        }
    }

    public Common.DTOPawn createPawnDTO((uint, uint) pos, List<Common.Direction> mooves) {
        Common.DTOPawn Pawn = new Common.DTOPawn() { startPos = pos, destPos = (null, null), mooves = mooves };
        return Pawn;
    }

    public Common.DTOWall createWallDTO((uint, uint) pos, (uint, uint) secondPos, Common.Direction dir, bool add) {
        Common.DTOWall Wall = new Common.DTOWall() { coord1 = pos, coord2 = secondPos, direction = dir, isAdd = add };
        return Wall;
    }

    public Common.DTOError createErrorDTO(int code) {
        Common.DTOError Error = new Common.DTOError() { errorCode = code };
        return Error;
    }

    public Common.DTOGameState createGameStateDTO(Player playerY, Player playerR, uint win, string currentPlayer) { 
        Common.DTOGameState.Player pY = new Common.DTOGameState.Player() { verticalWalls = playerY.getVerticalWallsCount(), horizontalWalls = playerY.getHorizontalWallsCount(), isPlaying = (currentPlayer == "Yellow") };
        Common.DTOGameState.Player pR = new Common.DTOGameState.Player() { verticalWalls = playerR.getVerticalWallsCount(), horizontalWalls = playerR.getHorizontalWallsCount(), isPlaying = (currentPlayer == "Red") };
        Common.DTOGameState GameState = new Common.DTOGameState() { yellowPlayer = pY, redPlayer = pR, winner = win };
        return GameState; 
    }
}
