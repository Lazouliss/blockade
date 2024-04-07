using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using blockade.Blockade_common;

public class Board : MonoBehaviour
{

    public const float LENGTH_TILE = 1;
    public Pawn selectedPawn;
    public GameObject wall;
    private Stack<GameObject> stackWall;

    // Start is called before the first frame update
    void Start()
    {

        stackWall = new Stack<GameObject>();
        /*
        Common.DTOPawn dto = new Common.DTOPawn();
        dto.startPos = (0, 0);
        dto.destPos = (1000, 1000);
        dto.mooves = (Common.Direction.UP, Common.Direction.RIGHT);


        this.moovePawn(dto);

        Common.DTOWall dtoWall = new Common.DTOWall();
        dtoWall.coord1 = (0,0);
        dtoWall.coord2 = (0,1);
        dtoWall.direction = Common.Direction.RIGHT;
        dtoWall.isAdd = true;
        actionWall(dtoWall);

        dtoWall.coord1 = (1, 1);
        dtoWall.coord2 = (2, 1);
        dtoWall.direction = Common.Direction.DOWN;
        dtoWall.isAdd = true;
        actionWall(dtoWall);

        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void moovePawn(Common.DTOPawn dto)
    {
        
        Pawn p = this.selectedPawn;

        for (int i = 0; i < dto.mooves.Count; i++) 
        {
            p.mooves(dto.mooves[i]);
        }
        //p.mooves(dto.mooves[0]);
        //p.mooves(dto.mooves[1]);

        //selectedPawn = null;

    }

    public void actionWall(Common.DTOWall dto)
    {
        Debug.Log("Début du actionWall");
        // test if dto.isAdd is initialized (if not, count as false) and if yes, check is value
        if (dto.isAdd.HasValue && dto.isAdd.Value)
            addWall(dto);
        else
            removeWall();

    }

    void addWall(Common.DTOWall dto)
    {
        Debug.Log("Début du addWall");
        int angle; 
        float x, z;

        
        if(dto.direction == Common.Direction.UP || dto.direction == Common.Direction.DOWN) { 

            angle = 90;
            x = Mathf.Min(dto.coord1.Item1, dto.coord2.Item1);
            if (dto.direction == Common.Direction.UP)
                z = dto.coord1.Item2 + (LENGTH_TILE);
            else
                z = dto.coord1.Item2;

        }
        else
        {

            angle = 0;
            z = Mathf.Min(dto.coord1.Item2, dto.coord2.Item2);
            if (dto.direction == Common.Direction.RIGHT)
                x = dto.coord1.Item1 + (LENGTH_TILE);
            else
                x = dto.coord1.Item1;

        }

        Quaternion rotation = Quaternion.Euler(0f, angle, 0f);

        GameObject newObject = Instantiate(wall, new Vector3(x, 0.5f, z), rotation);

        stackWall.Push( newObject );


    }

    void removeWall()
    {

        // Vérifiez d'abord si l'objet existe avant de le supprimer
        if (stackWall.Count > 0)
            Destroy(stackWall.Pop());
        else
        {
            Debug.LogWarning("La pile est vide, plus de mur à retirer");
        }

        

    }

}
