using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using blockade.Blockade_common;

public class GestionDTO : MonoBehaviour
{

    public const float LENGTH_TILE = 1;
    public Pawn selectedPawn;
    private Stack<GameObject> stackWall;
    public float speed = 0.5f;
    public GameObject wall;

    // Start is called before the first frame update
    void Start()
    {

        stackWall = new Stack<GameObject>();

        /*Common.DTOPawn dto = new Common.DTOPawn();
        dto.startPos = (0, 0);
        dto.destPos = (1000, 1000);

        List<Common.Direction> listMoove = new List<Common.Direction>();
        listMoove.Add(Common.Direction.UP);
        listMoove.Add(Common.Direction.RIGHT);
        dto.mooves = listMoove;


        this.moovePawn(dto);*/


        /*Common.DTOWall dtoWall = new Common.DTOWall();
        dtoWall.coord1 = (0,0);
        dtoWall.coord2 = (0,1);
        dtoWall.direction = Common.Direction.RIGHT;
        dtoWall.isAdd = true;
        actionWall(dtoWall);

        
        dtoWall.coord1 = (1, 1);
        dtoWall.coord2 = (2, 1);
        dtoWall.direction = Common.Direction.DOWN;
        dtoWall.isAdd = true;
        actionWall(dtoWall);*/


    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.C))
        {
            removeWall();
        }

    }

    public void moovePawn(Common.DTOPawn dto)
    {

        Pawn p = this.selectedPawn;

        foreach (Common.Direction direction in (List<Common.Direction>)(dto.mooves))
        {
            p.moove(direction);
        }

        //selectedPawn = null;

    }

    public void actionWall(Common.DTOWall dto)
    {

        if ((bool)(dto.isAdd))
            addWall(dto);
        else
            removeWall();

    }

    void addWall(Common.DTOWall dto)
    {

        int angle;
        float x, z;


        if (dto.direction == Common.Direction.UP || dto.direction == Common.Direction.DOWN)
        {

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

        Animator animator = newObject.GetComponent<Animator>();

        animator.SetTrigger("descendMur");

        stackWall.Push(newObject);


    }

    void removeWall()
    {

        // Vérifiez d'abord si l'objet existe avant de le supprimer
        if (stackWall.Count > 0)
        {
            GameObject wall = stackWall.Pop();


            Animator animator = wall.GetComponent<Animator>();
            animator.SetTrigger("monterMur");


        }
        else
        {
            Debug.LogWarning("La pile est vide, plus de mur à retirer");
        }

    }

}
