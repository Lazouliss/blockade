using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using blockade.Blockade_common;
using System;

namespace blockade.Blockade_IHM
{
    public class Pawn : MonoBehaviour
    {

        int x, y;
        int id_player;

        public Pawn(int x, int y)
        {
            this.x = x;
            this.y = y;
        }


        public void move(Common.Direction direction)
        {

            float length = ApplyDTO.LENGTH_TILE;

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

        //ABERKANE Doha & Thomas MONTIGNY (Bug fix)
        //creation d'un fonction qui va créer mon objet pion
        public static Pawn createPawn(Vector2Int position, string name, Board plateau, int id_player)
        {

            string path = "ToonyTinyPeople/TT_RTS/TT_RTS_standard_demo/prefab/TT_RTS_Demo_Character";
            if (id_player == 2)
            {
                path = "ToonyTinyPeople/TT_RTS/TT_RTS_standard_demo/prefab/TT_RTS_Demo_Character_Red";
            }
            //creation d'un objet pawnPrefab qui prendra le petit chevalier qui se trouve dans Resources
            GameObject pawnPrefab = Resources.Load<GameObject>(path);

            GameObject pawnObj = GameObject.Instantiate(pawnPrefab); // instantiation du prefab 
            pawnObj.transform.position = new Vector3(position.x, 0.5f, position.y); // positionne le GameObject dans l'espace en fonction des positions x & y
            if (id_player == 2)
            {
                Vector3 newRotation = new Vector3(0.0f, 180.0f, 0.0f);
                //cams.transform.eulerAngles = newRotation;
                pawnObj.transform.Rotate(0, 180, 0, Space.Self);
            }

            Pawn pawn = pawnObj.AddComponent<Pawn>(); // ajout d'un Pawn component pour le GameObject
            pawn.name = name; // attribution d'un nom
            pawn.x = position.x; // attribution position x
            pawn.y = position.y; // attribution position y
            pawn.id_player = id_player;

            pawnObj.name = name; // attribution d'un nom à mon objet pawnObj

            pawnObj.AddComponent<CapsuleCollider>(); // ajout d'une CapsuleCollider pour mon pawn
            pawnObj.AddComponent<PawnClickHandler>(); // ajout de mon PawnClickHandler pour gestion des cliques
            pawnObj.GetComponent<PawnClickHandler>().plateau = plateau;

            // change de parent pour prendre le plateau
            pawnObj.transform.SetParent(plateau.transform, false);

            return pawn;
        }

        public int GetID()
        {
            return id_player;
        }

        /*
        //ABERKANE Doha
        public static getPositionPawn(Pawn pawn){

            return pawn.transform.position;
        }*/

    }
}