using UnityEngine;
using blockade.Blockade_common;
using System.Collections.Generic;
//ABERKANE Doha
public class Plateau : MonoBehaviour
{
    //Board
    private GameObject case_plateau;
    private const int width = Common.MAP_WIDTH;
    private const int height = Common.MAP_HEIGHT;

    public IHM ihm;

    private Pawn selectedPawn;
    private Common.DTOPawn dtoPawn;

    /// <summary>
    /// Par Thomas MONTIGNY
    /// 
    /// Appelé par l'IHM au lancement de la partie, crée le plateau automatiquement
    /// 
    /// Publique
    /// </summary>
    /// <param name="nbWalls"></param>
    public void StartGame(int nbWalls)
    {
        Init_Plateau();
        Init_Walls(nbWalls);
    }

    //ABERKANE Doha
    //Initialisation du plateau
    private void Init_Plateau()
    {

        //creation d'une couleur 
        Color couleurCase;
        dtoPawn = InitDTOPawn();

        //boucle qui va créer les cases du plateau
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //creation du cube 
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                // TEST !!!!!!!!!!!!!!!!
                cube.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);

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
                    Pawn.createPawn(new Vector2Int(3, 3), "player1_Pion1", this, 1);
                    case_plateau.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.gray);

                }
                else if (x == 7 && y == 3)
                {
                    Pawn.createPawn(new Vector2Int(7, 3), "player1_Pion2", this, 1);
                    case_plateau.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.gray);
                }
                else if (x == 3 && y == 10)
                {
                    Pawn.createPawn(new Vector2Int(3, 10), "player2_Pion1", this, 2);
                    case_plateau.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.gray);
                }
                else if (x == 7 && y == 10)
                {
                    Pawn.createPawn(new Vector2Int(7, 10), "player2_Pion2", this, 2);
                    case_plateau.gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.gray);
                }
            }
        }

    }

    /// <summary>
    /// Par Thomas MONTIGNY
    ///
    /// Création de tous les murs le long du plateau, pour les 2 joueurs
    /// 
    /// Privée
    /// </summary>
    private void Init_Walls(int nb_walls)
    {
        Debug.Log("Creating walls");

        // pour que les murs ne soient pas tous collés en un bloc
        float spaceBetweenWalls = 0.5f;

        // TODO : RENDRE TOUT CELA PLUS PROPRE !!!!!!!!!!

        // Position de départ pour les murs placés verticalement
        Vector2 startPos = new Vector2(12, 0);
        Vector2 currentPos = startPos;
        // Position de départ pour les murs placés horizontalement
        Vector2 startPosHoriz = new Vector2(11, nb_walls*spaceBetweenWalls+1);
        Vector2 currentPosHoriz = startPosHoriz;

        // Pareil pour le joueur 2
        // Position de départ pour les murs placés verticalement
        Vector2 startPosP2 = new Vector2(-2, 13);
        Vector2 currentPosP2 = startPosP2;
        // Position de départ pour les murs placés horizontalement
        Vector2 startPosHorizP2 = new Vector2(-1, 13 - (nb_walls * spaceBetweenWalls + 1));
        Vector2 currentPosHorizP2 = startPosHorizP2;

        for (int i=0; i<nb_walls; i++)
        {
            // Player 1
            // Création d'un mur horizontal, aligné verticalement le long du plateau
            Wall.createWall(currentPos, 1, false, this);
            // Création d'un mur vertical, aligné horizontalement le long du plateau
            Wall.createWall(currentPosHoriz, 1, true, this);

            // Mise à jour des positions pour les prochains murs
            currentPos = new Vector2(currentPos.x, currentPos.y + spaceBetweenWalls);
            currentPosHoriz = new Vector2(currentPosHoriz.x + spaceBetweenWalls, currentPosHoriz.y);

            // Same for player 2 ???
            // Création d'un mur horizontal, aligné verticalement le long du plateau
            Wall.createWall(currentPosP2, 2, false, this);
            // Création d'un mur vertical, aligné horizontalement le long du plateau
            Wall.createWall(currentPosHorizP2, 2, true, this);

            // Mise à jour des positions pour les prochains murs
            currentPosP2 = new Vector2(currentPosP2.x, currentPosP2.y - spaceBetweenWalls);
            currentPosHorizP2 = new Vector2(currentPosHorizP2.x - spaceBetweenWalls, currentPosHorizP2.y);
        }
    }
        
    //ABERKANE Doha & Thomas MONTIGNY
    //fonction d'envoi des positions en dto
    public void SendDTO(Vector2 pos, bool isStartPos)
    {
        //dtoPawn = dto; //Màj du dto

        Debug.Log(pos + " " + (uint)pos[0] + " " + (uint)pos[1]);
        if (isStartPos)
        {
            dtoPawn.startPos = ((uint)pos[0], (uint)pos[1]);
        }
        else
        {
            dtoPawn.destPos = ((uint)pos[0], (uint)pos[1]);
        }

        //Vérifie si dtoPawn contient des valeurs de positions de type float
        if (dtoPawn.startPos != (1000, 1000) && dtoPawn.destPos != (1000, 1000))
        {
            // select the pawn on the board
            ihm.GetComponent<IHM>().board.selectedPawn = selectedPawn;

            // Test
            dtoPawn.mooves.Add(Common.Direction.UP);
            dtoPawn.mooves.Add(Common.Direction.UP);
            ihm.GetComponent<IHM>().sendDTO(dtoPawn);

            //IHM.sendDTOToLogic(dtoPawn); //appel de la fonction  sendDTOToLogic() pour envoyer les valeurs du DTO actuel

            // reset dto pawn
            dtoPawn = InitDTOPawn();
            // and remove selected pawn
            selectedPawn = null;
        }
    }

    /// <summary>
    /// Par Thomas MONTIGNY
    ///
    /// Selectionne le pion
    /// 
    /// Publique
    /// </summary>
    /// <param name="pawn"></param>
    public void SelectPawn(Pawn pawn)
    {
        this.selectedPawn = pawn;
    }

    /// <summary>
    /// Par Thomas MONTIGNY
    ///
    /// Initialise un dto pion
    /// 
    /// Privee
    /// </summary>
    /// <returns></returns>
    private Common.DTOPawn InitDTOPawn()
    {
        Common.DTOPawn dto = new Common.DTOPawn();

        dto.startPos = ((uint)1000, (uint)1000);
        dto.destPos = ((uint)1000, (uint)1000);
        dto.mooves = new List<Common.Direction>();

        return dto;
    }
}


