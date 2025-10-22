using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    // walls
    private uint horizontalWalls;
    private uint verticalWalls;

    private string playerName;
    private uint playerID;

    private uint[] casesGagnantes = new uint[2];
    private uint[] positionsPions = new uint[2];

    public Player(string name, uint id)
    {
        this.playerID = id;
        this.playerName = name;
        this.horizontalWalls = 9;
        this.verticalWalls = 9;
    }
    // Start is called before the first frame update
    public void SetplayerName(string playerName)
    {
        this.playerName = playerName;
    }
    public void SetplayerID(uint playerID)
    {
        this.playerID = playerID;
    }
    public uint getPlayerID()
    {
        return this.playerID;
    }
    public uint[] getWinningSquaresPosition()
    {
        return this.casesGagnantes;
    }
    public uint[] getPositionsPions()
    {
        return this.positionsPions;
    }
    public uint getHorizontalWallsCount()
    {
        return this.horizontalWalls;
    }
    public uint getVerticalWallsCount()
    {
        return this.verticalWalls;
    }
    public void retirerWallHorizontal()
    {
        this.horizontalWalls--;
    }
    public void retirerWallVertical()
    {
        this.verticalWalls--;
    }

    public void setWinningSquaresPosition(uint[] casesGagnantes)
    {
        this.casesGagnantes = casesGagnantes;
    }

    public void setPositionsPions(uint[] positionsPions)
    {
        this.positionsPions = positionsPions;
    }

    public void deplacerPion(uint casePionDepart, uint casePionArrivee)
    {
        Debug.Log("Pion moving from " + casePionDepart + " to " + casePionArrivee);
        for (int i = 0; i < this.positionsPions.Length; i++)
        {
            Debug.Log("Pion " + i + " is on case " + this.positionsPions[i]);
            if (this.positionsPions[i] == casePionDepart)
            {
                this.positionsPions[i] = casePionArrivee;
                Debug.Log("Pion moved from " + casePionDepart + " to " + casePionArrivee);
            }
        }
    }
    public void SetHorizontalWalls(uint horizontalWalls)
    {
        this.horizontalWalls = horizontalWalls;
    }
    public void SetVerticalWalls(uint verticalWalls)
    {
        this.verticalWalls = verticalWalls;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
