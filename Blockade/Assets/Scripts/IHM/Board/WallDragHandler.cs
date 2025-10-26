using UnityEngine;

namespace blockade.Blockade_IHM
{
    public class WallDragHandler : MonoBehaviour
    {
        private GameObject selectedWall;
        public Board board;
        public bool flag = true;

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Ex�cut�e toutes les frames
        /// </summary>
        void Update()
        {
            // D�place le mur avec le curseur tant qu'il est s�lectionn�
            if (selectedWall != null)
            {
                Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedWall.transform.position).z);
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
                selectedWall.transform.position = new Vector3(worldPosition.x, 1.25f, worldPosition.z);

                // Garder la possibilit� de faire tourner un mur ? (il faut changer l'attribut isVerti si c'est le cas, et supprimer le bon mur de l'espace de droite apr�s cela).
                /*
                if (Input.GetMouseButtonDown(1))
                {
                    selectedWall.transform.rotation = Quaternion.Euler(new Vector3(
                        selectedWall.transform.rotation.eulerAngles.x,
                        selectedWall.transform.rotation.eulerAngles.y + 90f,
                        selectedWall.transform.rotation.eulerAngles.z));
                }
                */
            }
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Deplacement du contenu de la fonction OnMouseDown() dans une ActionWall pour qu'elle 
        /// puisse etre appelee par une autre partie (pour par exemple jouer un coup par une IA)
        /// </summary>
        public void OnMouseDown()
        {
            if (flag) {
                ActionWall();
            }
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// S�lectionne le mur � d�plac� si ce n'est pas d�j� fait ou envoie un DTO pour essayer de le placer sinon.
        /// 
        /// Publique
        /// </summary>
        public void ActionWall()
        {
            // if no walls are selected
            if (selectedWall == null)
            {
                RaycastHit hit = CastRay();

                // if there is an object pointed by the mouse
                if (hit.collider != null)
                {
                    // and it has the "drag" tag
                    if (!hit.collider.CompareTag("drag"))
                    {
                        return;
                    }

                    // get the ihm
                    IHM ihm = hit.collider.gameObject.GetComponent<Wall>().GetBoard().ihm;

                    //Debug.Log(hit.collider.gameObject.GetComponent<Wall>().GetId() + " != " + ihm.GetCurrentPlayer() + " -> " + (hit.collider.gameObject.GetComponent<Wall>().GetId() != ihm.GetCurrentPlayer()));
                    //Debug.Log(ihm.GetPlayer(ihm.GetCurrentPlayer()).isPlacingWall);

                    // and if the player cant place walls or is trying to move a wall of the opponent
                    //!ihm.GetPlayer(ihm.GetCurrentPlayer()).isPlacingWall || 
                    if (hit.collider.gameObject.GetComponent<Wall>().GetId() != ihm.GetCurrentPlayer())
                    {
                        return;
                    }

                    // select the object and make it transparent
                    selectedWall = hit.collider.gameObject;
                    selectedWall.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                    Cursor.visible = false;
                }
            }
            // Send the dto to try to place the object
            else
            {
                // Get worldPosition using the position of the mouse
                Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedWall.transform.position).z);
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);

                // Select the wall on the board
                board.SelectWall(selectedWall);

                // Affichage de la position du mur puis appel de la fonction d'envoie de DTO
                Debug.Log("Wall current worldPosition : " + worldPosition);
                selectedWall.GetComponent<Wall>().sendDTOWall(worldPosition);
            }
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Fonction de destruction du mur selectionne (celui visible au bout du curseur).
        /// Executee seulement quand un mur est place, donc seulement s'il a pu etre place.
        /// 
        /// Publique
        /// </summary>
        public void UnSelectWall() 
        { 
            if (selectedWall != null)
            {
                // Destroy the wall
                Destroy(selectedWall);

                // D�selectionne l'objet et fait r�apparaitre le curseur
                selectedWall = null;
                Cursor.visible = true;
            } 
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// R�cup�re l'objet que le curseur pointe
        /// 
        /// Priv�e
        /// </summary>
        /// <returns></returns>
        private RaycastHit CastRay()
        {
            // Get mouse position on the screen
            Vector3 screenMousePosFar = new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                Camera.main.farClipPlane);
            Vector3 screenMousePosNear = new Vector3(
                Input.mousePosition.x,
                Input.mousePosition.y,
                Camera.main.nearClipPlane);

            // Get mouse position on the game / world
            Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
            Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);

            // Create a raycast and get all objects on the raycast
            RaycastHit hit;
            Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);

            return hit;
        }
    }
}
