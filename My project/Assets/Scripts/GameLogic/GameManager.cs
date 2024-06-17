using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using blockade.Blockade_common;
using blockade.Blockade_IHM;
using blockade.AI;
using blockade.Blockade_Online;

public class GameManager : MonoBehaviour
{
    private AlgoBoard algoBoard;
    private List<Player> players = new List<Player>();
    private Player playingPlayer;
    private DTOHandler dtoHandler = new DTOHandler();
    private IHM ihm;
    private BotAPI botAPI;
    private bool wallPlaced = false;
    private bool pawnMoved = false;
    public Online online;
    private object last_dto;

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
        botAPI = new BotAPI();
    }
    void Awake()
    {
        Debug.Log("awake game manager");
        ihm = GetComponent<IHM>();
        online = gameObject.AddComponent<Online>();
        online.lobbyClient.gmDtoSend = sendDTO;
    }
    private void togglePlayingPlayer()
    {
        playingPlayer = (playingPlayer == players[0]) ? players[1] : players[0];
    }
    public void sendDTO(object dto, bool isCurrentPlayer = true){
        switch (dto)
            {
                case Common.DTOWall dtoWall:

                if(ihm.GetTypePartie() == "JCE"){
                    botAPI.sendDTO(dtoWall);
                }

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
                if(ihm.GetTypePartie() == "ONLINE" && isCurrentPlayer){
                    online.sendAction(dtoWall);
                }
                last_dto = dtoWall;
                break;


                case Common.DTOPawn dtoPawn: 

                if(ihm.GetTypePartie() == "JCE"){
                    botAPI.sendDTO(dtoPawn);
                }

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
                    Common.DTOPawn dtoPawnToSend = dtoHandler.createPawnDTO(startPos, algoBoard.GetPath(res.Item2.ToArray()));
                    Common.DTOPawn dtoPawnReversed = dtoPawn;
                    dtoPawnReversed.mooves = dtoPawnToSend.mooves;
                    last_dto = dtoPawnReversed;
                    ihm.sendDTO(dtoPawnToSend);
                } else {
                    ihm.sendDTO(dtoHandler.createErrorDTO(1));
                    Debug.Log("Pawn impossible to place");
                    break;
                }
                pawnMoved = true;
                if(ihm.GetTypePartie() == "ONLINE" && isCurrentPlayer){
                    online.sendAction(dtoPawn);
                }
                check_winning();
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

                    if(ihm.GetTypePartie() == "JCE"){
                        botAPI.sendDTO(dtoHandler.createGameStateDTO(players[0], players[1], 0, playingPlayerString));
                        

                        (Common.DTOPawn, Common.DTOWall) dtosBot = botAPI.get_move(BotAPI.Difficulty.HARD); // TODO : put real difficulty here 
                        (uint, uint) startPos = dtosBot.Item1.startPos;
                        (uint?, uint?) endPos = dtosBot.Item1.destPos;
                        (bool, List<Square>, int) resBot = algoBoard.getChemin(((uint)startPos.Item1, (uint)startPos.Item2), ((uint)endPos.Item1, (uint)endPos.Item2));
                        
                        if (resBot.Item1)
                        {
                            uint casePionDepart = algoBoard.getPosition((uint)startPos.Item1, (uint)startPos.Item2);
                            uint casePionArrivee = algoBoard.getPosition((uint)endPos.Item1, (uint)endPos.Item2);
                            playingPlayer.deplacerPion(casePionDepart, casePionArrivee);
                            algoBoard.deplacerPion(((uint)startPos.Item1, (uint)startPos.Item2), ((uint)endPos.Item1, (uint)endPos.Item2));
                            ihm.sendDTO(dtoHandler.createPawnDTO(startPos, algoBoard.GetPath(resBot.Item2.ToArray())));
                        }
                        ihm.sendDTO(dtosBot.Item2);

                        uint botPlayerID = playingPlayer.getPlayerID();
                        bool botWon = algoBoard.checkWin(playingPlayer);
                        togglePlayingPlayer();
                        string playingPlayerStringBot = playingPlayer.getPlayerID() == 1 ? "Yellow" : "Red";

                        if (botWon){
                            ihm.sendDTO(dtoHandler.createGameStateDTO(players[0], players[1], botPlayerID, playingPlayerStringBot));
                        } else {
                            ihm.sendDTO(dtoHandler.createGameStateDTO(players[0], players[1], 0, playingPlayerStringBot));
                            botAPI.sendDTO(dtoHandler.createGameStateDTO(players[0], players[1], 0, playingPlayerStringBot));
                        }
                    }
                    
                }
                ihm.board.ChangeCaseTexture(algoBoard.GetValidPawnMoves(playingPlayer));
            }
    }


     public void AddPlayer(Player player)
     {
         players.Add(player);
     }


    public void check_winning()
    {
        if (algoBoard.checkWin(playingPlayer))
        {
            ihm.sendDTO(dtoHandler.createGameStateDTO(players[0], players[1], playingPlayer.getPlayerID(), ""));
        }
    }
    public List<Common.Direction> reverseDirections(List<Common.Direction> directions)
    {
        List<Common.Direction> reversedDirections = new List<Common.Direction>();
        foreach (Common.Direction direction in directions)
        {
            switch (direction)
            {
                case Common.Direction.UP:
                    reversedDirections.Add(Common.Direction.DOWN);
                    break;
                case Common.Direction.DOWN:
                    reversedDirections.Add(Common.Direction.UP);
                    break;
                case Common.Direction.LEFT:
                    reversedDirections.Add(Common.Direction.RIGHT);
                    break;
                case Common.Direction.RIGHT:
                    reversedDirections.Add(Common.Direction.LEFT);
                    break;
            }
        }
        return reversedDirections;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && ihm.GetTypePartie() == "JCE")
        {
            Debug.Log("Reversing DTO");
            switch (last_dto)
            {
                case Common.DTOWall dtoWall:
                    togglePlayingPlayer();
                    wallPlaced = false;
                    pawnMoved = true;
                    string playingPlayerString = playingPlayer.getPlayerID() == 1 ? "Yellow" : "Red";
                    ihm.sendDTO(dtoHandler.createGameStateDTO(players[0], players[1], 0, playingPlayerString));
                    Common.DTOWall dtoWallReversed = dtoWall;
                    dtoWallReversed.isAdd = false;
                    ihm.sendDTO(dtoWallReversed);
                    algoBoard.removeWall(dtoWall.coord1, dtoWall.coord2, dtoWall.direction);
                    if (dtoWall.direction == Common.Direction.LEFT || dtoWall.direction == Common.Direction.RIGHT) {
                        if(playingPlayer.getPlayerID() == 1) {
                            ihm.EditStackVerticalWall(ihm.GetCurrentPlayer(), Wall.createWall(new Vector2(11,5), 1, true, ihm.board, true));
                        } else {
                            ihm.EditStackVerticalWall(ihm.GetCurrentPlayer(), Wall.createWall(new Vector2(-1, 12), 2, true, ihm.board, true));
                        }
                    } else {
                        if(playingPlayer.getPlayerID() == 1) {
                            ihm.EditStackHorizontalWall(ihm.GetCurrentPlayer(), Wall.createWall(new Vector2(12, 8), 1, false, ihm.board, true));
                        } else {
                            ihm.EditStackHorizontalWall(ihm.GetCurrentPlayer(), Wall.createWall(new Vector2(-2, 13), 2, false, ihm.board, true));
                        }
                    }
                    last_dto = null;
                    break;
                case Common.DTOPawn dtoPawn:
                    Common.DTOPawn dtoPawnReversed = dtoPawn;
                    (uint, uint) start = dtoPawn.startPos;
                    (uint?, uint?) end = dtoPawn.destPos;
                    uint nouveauArrive = algoBoard.getPosition((uint)start.Item1, (uint)start.Item2);
                    uint nouveauDepar = algoBoard.getPosition((uint)end.Item1, (uint)end.Item2);
                    pawnMoved = false;
                    dtoPawnReversed.mooves = reverseDirections(dtoPawn.mooves);
                    dtoPawnReversed.startPos = ((uint)end.Item1, (uint)end.Item2);
                    dtoPawnReversed.destPos = ((uint)start.Item1, (uint)start.Item2);
                    playingPlayer.deplacerPion(nouveauDepar, nouveauArrive);
                    algoBoard.deplacerPion(((uint)end.Item1, (uint)end.Item2), ((uint)start.Item1, (uint)start.Item2));
                    print( dtoPawnReversed.startPos + " " + dtoPawnReversed.destPos + " " + dtoPawnReversed.mooves[0] + " " + dtoPawnReversed.mooves[1]);
                    ihm.sendDTO(dtoPawnReversed);
                    last_dto = null;
                    break;
                default:
                    break;
            }
        }
    }

}