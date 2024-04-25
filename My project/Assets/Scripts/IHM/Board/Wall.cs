using blockade.Blockade_common;
using UnityEngine;

namespace blockade.Blockade_IHM
{
    public class Wall : MonoBehaviour
    {
        private int id_player;
        private bool isVerti;
        private Board board;

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Fonction de création d'une instance de la classe Wall
        /// 
        /// Privée
        /// </summary>
        /// <param name="id_player">Identifiant du joueur propriétaire du mur</param>
        /// <param name="isVerti">Si le mur est vertical (vrai ou faux)</param>
        /// <param name="board">Lien vers le plateau</param>
        private void CreateWall(int id_player, bool isVerti, Board board)
        {
            this.id_player = id_player;
            this.isVerti = isVerti;
            this.board = board;
        }

        // Getters
        public int GetId() { return id_player; }
        public bool IsVerti() { return isVerti; }
        public Board GetBoard() { return board; }

        /// <summary>
        /// Par Thomas MONTIGNY
        ///
        /// Création d'un mur, cliquable seulement par un joueur (playerID, TODO), aux positions en argument
        /// </summary>
        public static void createWall(Vector2 pos, int id_player, bool isVerti, Board board)
        {
            // Create wall using prefab and instantiate it on the right position
            GameObject wallPrefab = Resources.Load<GameObject>("Wall");
            GameObject wall = Instantiate(wallPrefab);
            wall.transform.position = new Vector3(pos.x, 0.5f, pos.y);
            if (!isVerti)
            {
                wall.transform.rotation = Quaternion.Euler(0f, 90, 0f);
            }

            // TEMPORARY --> OR NEEDS TO BE USED FOR THE PLACEMENT TOO
            if (id_player == 2)
            {
                wall.GetComponent<MeshRenderer>().material.color = Resources.Load<Material>("WallMaterial_player2").color;
            }

            // Add DragHandler
            wall.AddComponent<WallDragHandler>();

            // Add script to wall
            wall.AddComponent<Wall>();
            wall.GetComponent<Wall>().CreateWall(id_player, isVerti, board);

            //Debug.Log(wall.GetComponent<Wall>().GetId_Player());

            // Add tag
            wall.tag = "drag";

            // change de parent pour prendre le plateau
            wall.transform.SetParent(board.transform, false);
        }

        /// <summary>
        /// Par Wassim BOUKHARI
        /// 
        /// Detruit le mur
        /// 
        /// </summary>
        public void detruireMur()
        {

            Destroy(gameObject);

        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Création d'un DTOWall à partir des positions choisie pour placer le mur
        /// 
        /// Publique (pour être appelée au moment où le mur est placé)
        /// </summary>
        /// <param name="worldPos"></param>
        public void sendDTOWall(Vector3 worldPos)
        {
            Debug.Log("Create DTO Wall");

            // Création du DTO
            Common.DTOWall wall = new Common.DTOWall();

            // Si le mur est vertical initialisation des positions en ajoutant 1 au z de la seconde coordonnée
            if (this.isVerti)
            {
                //Debug.Log("Ce mur est vertical");
                wall.coord1 = ((uint)worldPos.x, (uint)worldPos.z);
                wall.coord2 = ((uint)worldPos.x, (uint)worldPos.z + 1);
                wall.direction = Common.Direction.RIGHT;
            }
            // Si le mur est horizontal initialisation des positions en ajoutant 1 au x de la seconde coordonnée
            else
            {
                wall.coord1 = ((uint)worldPos.x, (uint)worldPos.z);
                wall.coord2 = ((uint)worldPos.x + 1, (uint)worldPos.z);
                wall.direction = Common.Direction.UP;
            }

            // for tests --> apply dto without checking if its legal
            wall.isAdd = true;
            board.ihm.sendDTO(wall);

            // Send DTO to game logic
            //board.ihm.sendDTOToLogic(wall);
        }
    }
}