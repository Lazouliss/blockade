using UnityEngine;
using blockade.Blockade_common;
using System.Collections;

namespace blockade.Blockade_IHM
{
    public class Pawn : MonoBehaviour
    {

        uint x, y;
        int id_player;

        public Pawn(uint x, uint y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Getters & Setters des positions du pion
        /// 
        /// Publique
        /// </summary>
        /// <returns></returns>
        public (uint,uint) GetPosPawn() { return (x, y); }
        public void SetPosPawn(uint x, uint y) { this.x = x; this.y = y; }

        /// <summary>
        /// Par Wassim BOUKHARI
        ///  
        /// Déplace le pion selon la 
        /// direction en argument
        /// 
        /// </summary>
        /// <param name="dto"></param>
        public IEnumerator move(Common.Direction direction)
        {

            float length = ApplyDTO.LENGTH_TILE;

            float speed = 5.0f; // Vitesse de déplacement
            float time = 0;

            Vector3 start = transform.position;
            Quaternion startRotation = transform.rotation;



            Vector3 target = start;
            Quaternion targetRotation;
            switch (direction)
            {
                case Common.Direction.UP:
                    target += new Vector3(0, 0, length);
                    targetRotation = Quaternion.Euler(0, 0, 0);

                    break;
                case Common.Direction.DOWN:
                    target += new Vector3(0, 0, -length);
                    targetRotation = Quaternion.Euler(0, 180, 0);
                    break;
                case Common.Direction.LEFT:
                    target += new Vector3(-length, 0, 0);
                    targetRotation = Quaternion.Euler(0, -90, 0);
                    break;
                case Common.Direction.RIGHT:
                    target += new Vector3(length, 0, 0);
                    targetRotation = Quaternion.Euler(0, 90, 0);
                    break;
                default:
                    target += new Vector3(0, 0, 0);
                    targetRotation = Quaternion.Euler(0, 0, 0);
                    break;

            }


            Debug.Log("Going to turn the pawn " + transform.name + ", Start : " + startRotation + ", Target : " + targetRotation);

            while (time < 1.0f)
            {
                // Augmenter le temps de lerp basé sur le temps écoulé multiplié par la vitesse
                time += Time.deltaTime * speed;

                // Effectuer le lerp
                transform.rotation = Quaternion.Lerp(startRotation, targetRotation, time);

                //Debug.Log("Time : " + time + ", deltaTime :" + Time.deltaTime + " and position : " + transform.position);

                // Attendre la fin de la frame avant de continuer
                yield return null;
            }

            time = 0;
            Debug.Log("Going to move the pawn " + transform.name + ", Start : " + start + ", Target : " + target);

            while (time < 1.0f)
            {
                // Augmenter le temps de lerp basé sur le temps écoulé multiplié par la vitesse
                time += Time.deltaTime * speed;

                // Effectuer le lerp
                transform.position = Vector3.Lerp(start, target, time);

                //Debug.Log("Time : " + time + ", deltaTime :" + Time.deltaTime + " and position : " + transform.position);

                // Attendre la fin de la frame avant de continuer
                yield return null;
            }

            // Assurez-vous que la position finale est exactement la position cible
            transform.position = target;
            //Debug.Log("Final position : " + transform.position);
        }

        //ABERKANE Doha & Thomas MONTIGNY (Bug fix)
        //creation d'un fonction qui va créer mon objet pion
        public static GameObject createPawn(Vector2Int position, string name, Board plateau, int id_player)
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
            pawn.x = (uint)position.x; // attribution position x
            pawn.y = (uint)position.y; // attribution position y
            pawn.id_player = id_player;

            pawnObj.name = name; // attribution d'un nom à mon objet pawnObj

            pawnObj.AddComponent<CapsuleCollider>(); // ajout d'une CapsuleCollider pour mon pawn
            pawnObj.AddComponent<PawnClickHandler>(); // ajout de mon PawnClickHandler pour gestion des cliques
            pawnObj.GetComponent<PawnClickHandler>().plateau = plateau;

            // change de parent pour prendre le plateau
            pawnObj.transform.SetParent(plateau.transform, false);

            return pawnObj;
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