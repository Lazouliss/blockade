using UnityEngine;
using blockade.Blockade_common;
using System.Collections.Generic;
using System;
using System.Collections;

namespace blockade.Blockade_IHM
{
    //ABERKANE Doha
    public class Board : MonoBehaviour
    {
        //Board
        private GameObject case_plateau;
        private const int width = Common.MAP_WIDTH;
        private const int height = Common.MAP_HEIGHT;

        public IHM ihm;

        private Pawn selectedPawn;
        private Common.DTOPawn dtoPawn;

        public GameObject selectedWall;

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

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Getter et Setter pour selectedWall
        /// 
        /// Publiques
        /// </summary>
        /// <returns></returns>
        public GameObject GetSelectedWall() { return selectedWall; }
        public void SelectWall(GameObject wall) { selectedWall = wall; }

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
                    //creation des cases 
                    
                    //modélisation de la case
                    string path = "Mini-dungeon/kenney_mini-dungeon/Models/FBX format/rocks";
                   
                    GameObject casePrefab = Resources.Load<GameObject>(path);
                    GameObject caseObj = GameObject.Instantiate(casePrefab); // instantiation du prefab 
                    
                    caseObj.transform.localScale = new Vector3(0.95f, 0.9f, 1f);
                    //afficher les coordonnées sur le nom des cases
                    caseObj.name = (x, y).ToString();

                    //affectation d'un collider pour qu'il puisse être clickable
                    caseObj.AddComponent<BoxCollider>();

                    //creation de la case de position 
                    Vector3 position = new Vector3(x, 0f, y);

                    //intantiation de la case à la case correspondante dans la case_plateau
                    GameObject case_plateau = Instantiate(caseObj, position, Quaternion.identity);
                    case_plateau.transform.SetParent(transform);
                    Destroy(caseObj);

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
            Vector2 startPosHoriz = new Vector2(11, nb_walls * spaceBetweenWalls + 1);
            Vector2 currentPosHoriz = startPosHoriz;

            // Pareil pour le joueur 2
            // Position de départ pour les murs placés verticalement
            Vector2 startPosP2 = new Vector2(-2, 13);
            Vector2 currentPosP2 = startPosP2;
            // Position de départ pour les murs placés horizontalement
            Vector2 startPosHorizP2 = new Vector2(-1, 13 - (nb_walls * spaceBetweenWalls + 1));
            Vector2 currentPosHorizP2 = startPosHorizP2;

            for (int i = 0; i < nb_walls; i++)
            {
                // Player 1
                // Création d'un mur horizontal, aligné verticalement le long du plateau
                Wall.createWall(currentPos, 1, false, this);
                // Création d'un mur vertical, aligné horizontalement le long du plateau
                Wall.createWall(currentPosHoriz, 1, true, this);

                // Mise à jour des positions pour les prochains murs
                currentPos = new Vector2(currentPos.x, currentPos.y + spaceBetweenWalls);
                currentPosHoriz = new Vector2(currentPosHoriz.x + spaceBetweenWalls, currentPosHoriz.y);

                // Player 2
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
            if (isStartPos)
            {
                dtoPawn.startPos = ((uint)pos[0], (uint)pos[1]);
            }
            else if (dtoPawn.startPos != (1000, 1000))
            {
                dtoPawn.destPos = ((uint)pos[0], (uint)pos[1]);
            }

            //Vérifie si dtoPawn contient des valeurs de positions de type float
            if (dtoPawn.startPos != (1000, 1000) && dtoPawn.destPos != (1000, 1000))
            {
                // select the pawn on the board
                ihm.GetComponent<IHM>().gestionDTO.selectedPawn = selectedPawn;

                // Test
                /*
                dtoPawn.mooves.Add(Common.Direction.UP);
                dtoPawn.mooves.Add(Common.Direction.UP);
                ihm.GetComponent<IHM>().sendDTO(dtoPawn);
                */

                ihm.GetComponent<IHM>().sendDTOToLogic(dtoPawn); //appel de la fonction  sendDTOToLogic() pour envoyer les valeurs du DTO actuel
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

        public void ForgetSelectedPawn()
        {
            // reset dto pawn
            dtoPawn = InitDTOPawn();
            // and remove selected pawn
            selectedPawn = null;
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

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Supprime le plateau
        /// </summary>
        public void ClearBoard()
        {
            for (int i = 0; i< this.gameObject.transform.childCount; i++)
            {
                Debug.Log("Destroying object : " + this.gameObject.transform.GetChild(i).gameObject);
                Destroy(this.gameObject.transform.GetChild(i).gameObject);
            }

            // TODO : reset les cams
        }
    }
}