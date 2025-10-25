using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square
{
    // id of the case
    private uint idCase;

    // inner cases metadata
    private Square upperSquare;
    private Square lowerSquare;
    private Square leftSquare;
    private Square rightSquare;

    // id of the player that has the pawn
    private uint? playerID;

    private bool winningSquare; //Modifier pour correspondre aux deux winning square des joueurs

    public Square(uint idCase)
    {
        this.idCase = idCase;
    }

    public bool isWinningSquare()
    {
        return winningSquare;
    }

    public uint getIdCase() { 
        return idCase; 
    }

    public uint? getPlayerID() {
        return playerID;
    }

    public bool HasPlayer() {
        return playerID > 0;
    }

    public Square getUpperSquare() { 
        return upperSquare; 
    }

    public Square getLowerSquare() { 
        return lowerSquare; 
    }

    public Square getLeftSquare() { 
        return leftSquare; 
    }

    public Square getRightSquare() { 
        return rightSquare; 
    }

    public void setIdSquare(uint idCase) {
        this.idCase = idCase;
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
    public void SetPlayerID(uint newPlayerID)
    {
        playerID = newPlayerID;
    }
    public void SetWinningSquare(bool winningSquare)
    {
        this.winningSquare = winningSquare;
    }
    public bool getWinningSquare()
    {
        return this.winningSquare;
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
