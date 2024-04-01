using UnityEngine;
using UnityEngine.EventSystems;
using blockade.Blockade_common;
using System.Linq;
using UnityEngine.UI;

//ABERKANE Doha
public class Plateau : MonoBehaviour
{
    //Board
    private GameObject case_plateau;
    private int width = 11;
    private int height = 14;

    //Pawns
    private Common.DTOPawn DTO_joueur1_pion1, DTO_joueur1_pion2;
    private Common.DTOPawn DTO_joueur2_pion1, DTO_joueur2_pion2;
    private Pawn selectedPawn;

    //Walls
    public GameObject murHorizontalPrefab;
    public GameObject murVerticalPrefab;

    void Start()
    {
        Init_Plateau();
    }

    //Initialisation du plateau
    public void Init_Plateau()
    {

         //initialisation des murs du plateau droite & gauche
        Init_Walls_Right();
        Init_Walls_Left();
        
        //initialisation des pions de départ
        //pour le joueur 1

        DTO_joueur1_pion1.startPos = (3, 3);
        DTO_joueur1_pion2.startPos = (7, 3);

        //pour le joueur 2
        DTO_joueur2_pion1.startPos = (3, 10);
        DTO_joueur2_pion2.startPos = (7, 10);

        //creation d'une couleur 
        Color couleurCase;

        //boucle qui va créer les cases du plateau
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //creation du cube 
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                //afficher les coordonnées sur le nom des cubes
                cube.name = (x, y).ToString();

                //affectation d'un collider pour qu'il puisse être clickable
                cube.AddComponent<BoxCollider>();

                //affection d'une texture
                var cubeRenderer = cube.GetComponent<Renderer>();

                //couleur de la case mise en noir
                couleurCase = Color.black;
                cubeRenderer.material.SetColor("_Color", couleurCase);

                //creation de la case de position 
                Vector3 position = new Vector3(x, 0f, y);

                //intantiation du cube à la case correspondante dans la plateau
                GameObject plateau = Instantiate(cube, position, Quaternion.identity);
                plateau.transform.SetParent(transform);

                CaseClickHandler clickHandler = plateau.AddComponent<CaseClickHandler>();//ajout d'un click handler pour les cases du plateau
                clickHandler.plateau = this;//référence de du plateau à chaque CaseClickHandler



                //initialisation des pions dans leurs cases de départ
                if (x == 3 && y == 3)
                {
                    Pawn pawn1 = Pawn.createPawn(new Vector2Int(3, 3), "Joueur1_Pion1", this);
                    //PawnClickHandler PclickHandler = pawn1.AddComponent<PawnClickHandler>();
                    //PclickHandler.plateau = this;
                }
                else if (x == 7 && y == 3)
                {
                    Pawn pawn2 = Pawn.createPawn(new Vector2Int(7, 3), "Joueur1_Pion2", this);
                    //PawnClickHandler PclickHandler = pawn2.AddComponent<PawnClickHandler>();
                    //PclickHandler.plateau = this;
                }
                else if (x == 3 && y == 10)
                {
                    Pawn pawn3 = Pawn.createPawn(new Vector2Int(3, 10), "Joueur2_Pion1", this);
                    //PawnClickHandler PclickHandler = pawn3.AddComponent<PawnClickHandler>();
                    //PclickHandler.plateau = this;
                }
                else if (x == 7 && y == 10)
                {
                    Pawn pawn4 = Pawn.createPawn(new Vector2Int(7, 10), "Joueur2_Pion2", this);
                    //PawnClickHandler PclickHandler = pawn4.AddComponent<PawnClickHandler>();
                    //PclickHandler.plateau = this;
                }
            }
        }
    }
   
    //Initialisation des murs côtés droits
    public void Init_Walls_Right()
    {
        //murs verticaux à droite du plateau
        for (int i = 0; i < 9; i++)
        {
            int x = width + 6; // Position X à côté de l'extrémité droite du plateau
            int y = 2 * i + 2;
            Vector3 position = new Vector3(x, 0.5f, y);
            GameObject murVertical = Instantiate(murVerticalPrefab, position, Quaternion.identity, transform);
            murVertical.name = "Joeur1_MurVertical_" + i;

            WallClickHandler clickHandler = murVertical.AddComponent<WallClickHandler>();
            clickHandler.plateau = this;

        }
        //murs horizontaux à droite du plateau

        for (int i = 0; i < 9; i++)
        {
            int x = width + 4;
            int y = 2 * i + 1;
            Vector3 position = new Vector3(x, 0.5f, y);
            GameObject murHorizontal = Instantiate(murHorizontalPrefab, position, Quaternion.identity, transform);
            murHorizontal.name = "Joeur1_MurHorizontal_" + i;

            WallClickHandler clickHandler = murHorizontal.AddComponent<WallClickHandler>();
            clickHandler.plateau = this;
        }


    }
    
    //Initialisation des murs côtés gauches
    public void Init_Walls_Left()
    {
        //murs verticaux à gauche du plateau
        for (int i = 0; i < 9; i++)
        {
            int x = -2; // Position X à côté de l'extrémité droite du plateau
            int y = 2 * i + 2;
            Vector3 position = new Vector3(x, 0.5f, y);
            GameObject murVertical = Instantiate(murVerticalPrefab, position, Quaternion.identity, transform);
            murVertical.name = "Joeur2_MurVertical_" + i;

            WallClickHandler clickHandler = murVertical.AddComponent<WallClickHandler>();
            clickHandler.plateau = this;

        }
        //murs horizontaux à gauche du plateau

        for (int i = 0; i < 9; i++)
        {
            int x = -3;
            int y = 2 * i + 1;
            Vector3 position = new Vector3(x, 0.5f, y);
            GameObject murHorizontal = Instantiate(murHorizontalPrefab, position, Quaternion.identity, transform);
            murHorizontal.name = "Joeur2_MurHorizontal_" + i;

            WallClickHandler clickHandler = murHorizontal.AddComponent<WallClickHandler>();
            clickHandler.plateau = this;
        }


    }
}


