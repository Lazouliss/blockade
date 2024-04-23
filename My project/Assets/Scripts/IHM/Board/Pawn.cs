using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using blockade.Blockade_common;

public class Pawn : MonoBehaviour
{

    int x, y;
    public float speed = 5.0f; // Vitesse de déplacement


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

    public void moove(Common.Direction direction)
    {

    

        Vector3 orientation;
        switch (direction)
        {
            case Common.Direction.UP:
                orientation = new Vector3(0, 0, 1);
                break;
            case Common.Direction.DOWN:
                orientation = new Vector3(0, 0, -1);
                break;
            case Common.Direction.LEFT:
                orientation = new Vector3(-1, 0, 0);
                break;
            case Common.Direction.RIGHT:
                orientation = new Vector3(1, 0, 0);
                break;
            default:
                orientation = new Vector3(0,0,0);
                break;
                
        }

        transform.position += orientation;

    }

}
