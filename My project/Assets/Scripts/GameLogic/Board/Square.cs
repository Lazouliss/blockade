using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    // id of the case
    private int idCase;

    // inner cases metadata
    private Square upperSquare;
    private Square lowerSquare;
    private Square leftSquare;
    private Square rightSquare;

    // id of the player that has the pawn
    private int playerID;

    public Square(int idCase)
    {
        this.idCase = idCase;
    }

    public int getIdCase() { 
        return idCase; 
    }
    // Setter for upperSquare
    public void SetUpperSquare(Square newUpperSquare)
    {
        upperSquare = newUpperSquare;
    }

    // Setter for lowerSquare
    public void SetLowerSquare(Square newLowerSquare)
    {
        lowerSquare = newLowerSquare;
    }

    // Setter for leftSquare
    public void SetLeftSquare(Square newLeftSquare)
    {
        leftSquare = newLeftSquare;
    }

    // Setter for rightSquare
    public void SetRightSquare(Square newRightSquare)
    {
        rightSquare = newRightSquare;
    }

    // Setter for playerID
    public void SetPlayerID(int newPlayerID)
    {
        playerID = newPlayerID;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
