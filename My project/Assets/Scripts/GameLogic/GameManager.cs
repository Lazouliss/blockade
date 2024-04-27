 using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;

public class GameManager : MonoBehaviour
{
    private AlgoBoard algoBoard;
    private List<Player> players = new List<Player>();
    private DTOHandler dtoHandler = new DTOHandler();

    public GameManager()
    {       
        Debug.Log("init game manager");
        Player playerY = new Player("Babime", 1);
        Player playerR = new Player("Ashlad", 2);
        algoBoard = new AlgoBoard(playerY, playerR);
        algoBoard.initBoard();
    }

    private void Awake()
    {
        
        Debug.Log("Start game");
        StartGame();
    }

    public void sendDTO(object dto){
        // TODO
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