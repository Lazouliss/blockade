using UnityEngine;
using UnityEngine.EventSystems;
using blockade.Blockade_common;
using System.Linq;
using UnityEngine.UI;
using System.Collections.Generic;
//ABERKANE Doha
public class Plateau : MonoBehaviour
{
    //Board
    private GameObject case_plateau;
    public int width = 11;
    public int height = 14;


    private Pawn selectedPawn;
    private Common.DTOPawn dtoPawn;
    //ABERKANE Doha

    void Start()
    {
        Init_Plateau();
    }
    //ABERKANE Doha
    //Initialisation du plateau
    public void Init_Plateau()
    {

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

                //intantiation du cube à la case correspondante dans la case_plateau
                GameObject case_plateau = Instantiate(cube, position, Quaternion.identity);
                case_plateau.transform.SetParent(transform);
                Destroy(cube);

                CaseClickHandler clickHandler = case_plateau.AddComponent<CaseClickHandler>();//ajout d'un click handler pour les cases du case_plateau
                clickHandler.plateau = this;//référence du plateau à chaque CaseClickHandler



                //initialisation des pions dans leurs cases de départ
                if (x == 3 && y == 3)
                {
                    Pawn pawn1 = Pawn.createPawn(new Vector2Int(3, 3), "player1_Pion1", this);

                }
                else if (x == 7 && y == 3)
                {
                    Pawn pawn2 = Pawn.createPawn(new Vector2Int(7, 3), "player1_Pion2", this);

                }
                else if (x == 3 && y == 10)
                {
                    Pawn pawn3 = Pawn.createPawn(new Vector2Int(3, 10), "player2_Pion1", this);

                }
                else if (x == 7 && y == 10)
                {
                    Pawn pawn4 = Pawn.createPawn(new Vector2Int(7, 10), "player2_Pion2", this);

                }
            }
        }

    }

    //ABERKANE Doha
    //fonction d'envoi des positions en dto
    public void SendDTO(Common.DTOPawn dto)
    {
        dtoPawn = dto; //Màj du dto

        //Vérifie si dtoPawn contient des valeurs de positions de type float
        if (dtoPawn.startPos != (0.0f, 0.0f) && dtoPawn.destPos != (0.0f, 0.0f))
        {
            //IHM.sendDTOToLogic(dtoPawn); //appel de la fonction  sendDTOToLogic() pour envoyer les valeurs du DTO actuel
        }
    }
}


