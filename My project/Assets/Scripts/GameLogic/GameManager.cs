using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using blockade.Blockade_common;
using blockade.Blockade_IHM;

public class GameManager : MonoBehaviour
{
    private AlgoBoard algoBoard;
    private List<Player> players = new List<Player>();
    private Player playingPlayer;
    private DTOHandler dtoHandler = new DTOHandler();
    private IHM ihm;
    private bool wallPlaced = false;
    private bool pawnMoved = false;

    public GameManager()
    {       
        Debug.Log("init game manager");
        Player playerY = new Player("Babime", 1);
        Player playerR = new Player("Ashlad", 2);
        AddPlayer(playerY);
        AddPlayer(playerR);
        playingPlayer = players[0];
        algoBoard = new AlgoBoard(playerY, playerR);
        algoBoard.initBoard();
    }
    void Awake()
    {
        Debug.Log("awake game manager");
        ihm = GetComponent<IHM>();
    }
    private void togglePlayingPlayer()
    {
        playingPlayer = (playingPlayer == players[0]) ? players[1] : players[0];
    }
    public void sendDTO(object dto){
        switch (dto)
            {
                case Common.DTOWall dtoWall:
                if (wallPlaced || dtoWall.coord1.Item1 > 10 || dtoWall.coord1.Item2 > 14 || dtoWall.coord2.Item1 > 10 || dtoWall.coord2.Item2 > 14 || dtoWall.coord1.Item1 < 0 || dtoWall.coord1.Item2 < 0 || dtoWall.coord2.Item1 < 0 || dtoWall.coord2.Item2 < 0){
                    ihm.sendDTO(dtoHandler.createErrorDTO(0));
                    Debug.Log("Wall impossible to place");
                    break;
                }
                
                bool canPlaceWall = (algoBoard.IsWallplaceable(dtoWall, players[0]) && algoBoard.IsWallplaceable(dtoWall, players[1]));
                if (canPlaceWall){
                    algoBoard.addWall(dtoWall.coord1, dtoWall.coord2, dtoWall.direction);
                } else {
                    ihm.sendDTO(dtoHandler.createErrorDTO(0));
                    Debug.Log("Wall impossible to place");
                    break;
                }

                if(dtoWall.direction == Common.Direction.LEFT || dtoWall.direction == Common.Direction.RIGHT){
                    playingPlayer.retirerWallHorizontal();
                } else {
                    playingPlayer.retirerWallVertical();
                }
                ihm.sendDTO(dtoHandler.createWallDTO(dtoWall.coord1, dtoWall.coord2, dtoWall.direction, canPlaceWall));
                wallPlaced = true;
                break;


                case Common.DTOPawn dtoPawn: 
                if (pawnMoved){
                    ihm.sendDTO(dtoHandler.createErrorDTO(0));
                    break;
                }
                (uint, uint) startPos = dtoPawn.startPos;
                (uint?, uint?) endPos = dtoPawn.destPos;
                (bool, List<Square>, int) res = algoBoard.getChemin(((uint)startPos.Item1, (uint)startPos.Item2), ((uint)endPos.Item1, (uint)endPos.Item2));
                
                if (res.Item1)
                {
                    uint casePionDepart = algoBoard.getPosition((uint)startPos.Item1, (uint)startPos.Item2);
                    uint casePionArrivee = algoBoard.getPosition((uint)endPos.Item1, (uint)endPos.Item2);
                    playingPlayer.deplacerPion(casePionDepart, casePionArrivee);
                    algoBoard.deplacerPion(((uint)startPos.Item1, (uint)startPos.Item2), ((uint)endPos.Item1, (uint)endPos.Item2));
                    ihm.sendDTO(dtoHandler.createPawnDTO(startPos, algoBoard.GetPath(res.Item2.ToArray())));
                } else {
                    ihm.sendDTO(dtoHandler.createErrorDTO(1));
                    Debug.Log("Pawn impossible to place");
                    break;
                }
                pawnMoved = true;
                break;

                default: Debug.Log("error during applying dto"); break;
            }
            if (wallPlaced && pawnMoved){
                uint playingPlayerID = playingPlayer.getPlayerID();
                bool playerWon = algoBoard.checkWin(playingPlayer);
                togglePlayingPlayer();
                wallPlaced = false;
                pawnMoved = false;
                string playingPlayerString = playingPlayer.getPlayerID() == 1 ? "Yellow" : "Red";
                if (playerWon){
                    ihm.sendDTO(dtoHandler.createGameStateDTO(players[0], players[1], playingPlayerID, playingPlayerString));
                } else {
                    ihm.sendDTO(dtoHandler.createGameStateDTO(players[0], players[1], 0, playingPlayerString));
                }
            }
    }


     public void AddPlayer(Player player)
     {
         players.Add(player);
     }

    public void StartGame()
        {
            //TESTINGS
            (uint, uint) startPos = (0,2);
            (uint, uint) endPos = (0,4);
            (bool, List<Square>, int) res = algoBoard.getChemin(startPos, endPos);         
            if (res.Item1)
            {
                //ihm.sendDTO(dtoHandler.createPawnDTO(startPos, algoBoard.GetPath(res.Item2.ToArray())));
            }


        // TODO: Implement the game loop here

        }

//     // TODO: Add more methods for game logic, such as handling player turns, checking for game over, etc.
}