using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlgoBoard : MonoBehaviour
{
    private int id;
    private Square[] cases = new Square[154];

    public AlgoBoard() {
        for (int i = 0; i < cases.Length; i++)
        {
            cases[i] = new Square(i);
        }
        AssignNeighbors();
    }
    private void AssignNeighbors()
    {
        for(int i = 0; i < this.cases.Length; i++)
        {
            int row = i / 11;
            int col = i % 11;
            int index = 0;

             // Lower Square
            if (row > 0 && row <= 13)
            {
                index = i - 11;
                if(index < 0)
                    this.cases[i].SetLowerSquare(cases[-index].getIdCase());
                else
                    this.cases[i].SetLowerSquare(cases[index].getIdCase());
            }
            else
                this.cases[i].SetLowerSquare(-1);

            // Upper Square  
            if (row < 13 && row >= 0)
                this.cases[i].SetUpperSquare(cases[i + 11].getIdCase());           
            else
                this. cases[i].SetUpperSquare(-1);

            // Left Square
            if (col > 0 && col <= 10)
                this.cases[i].SetLeftSquare(cases[i - 1].getIdCase());
            else
                this.cases[i].SetLeftSquare(-1);

            // Right Square
            if (col < 10 && col >= 0)
                this.cases[i].SetRightSquare(cases[i + 1].getIdCase());
            else
                this.cases[i].SetRightSquare(-1);
        }
    }

    public void movePawn()
    {
        // TODO
    }

    public void placeWall()
    {
        // TODO
    }

    public void checkWallPlacement()
    {
        // TODO
    }

    public void checkMove() 
    {
        // TODO
    }

    public void checkDTO()
    {
        // TODO
    }

    public void initBoard()
    {
        // TODO
    }

    public void checkWin()
    {
        // TODO
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
