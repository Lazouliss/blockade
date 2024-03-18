using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // walls
    private int horizontalWalls;
    private int verticalWalls;

    private string playerName;
    private int playerID;

    public Player(string name, int id)
    {
        this.playerID = id;
        this.playerName = name;
        this.horizontalWalls = 9;
        this.verticalWalls = 9;
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
