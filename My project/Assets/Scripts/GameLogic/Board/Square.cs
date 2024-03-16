using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    // id of the case
    private int idCase;

    // inner cases metadata
    private int upperSquare;
    private int lowerSquare;
    private int leftSquare;
    private int rightSquare;

    // id of the player that has the pawn
    private int playerID;

    private bool horizontalWall; 
    private bool verticalWall;
    public Square(int idCase)
    {
        this.idCase = idCase;
        this.horizontalWall = false;
        this.verticalWall = false;
    }

    public int getIdCase() { 
        return idCase; 
    }
    // Setter for upperSquare
    public void SetUpperSquare(int newUpperSquare)
    {
        upperSquare = newUpperSquare;
    }

    // Setter for lowerSquare
    public void SetLowerSquare(int newLowerSquare)
    {
        lowerSquare = newLowerSquare;
    }

    // Setter for leftSquare
    public void SetLeftSquare(int newLeftSquare)
    {
        leftSquare = newLeftSquare;
    }

    // Setter for rightSquare
    public void SetRightSquare(int newRightSquare)
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
