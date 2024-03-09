using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using blockade.Blockade_common;

public class Pawn : MonoBehaviour
{

    int x, y;

    public Pawn(int x, int y)
    {
        
        this.x = x;
        this.y = y;

    }

    // Start is called before the first frame update
    void Start()
    {

 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void mooves(Common.Direction direction)
    {

        float length = Board.LENGTH_TILE;

        Vector3 movement;
        switch (direction)
        {
            case Common.Direction.UP:
                movement = new Vector3(0, 0, length);
                break;
            case Common.Direction.DOWN:
                movement = new Vector3(0, 0, -length);
                break;
            case Common.Direction.LEFT:
                movement = new Vector3(-length, 0, 0);
                break;
            case Common.Direction.RIGHT:
                movement = new Vector3(length, 0, 0);
                break;
            default:
                movement = new Vector3(0, 0, 0);
                break;
                
        }

        transform.position += movement;

    }

}
