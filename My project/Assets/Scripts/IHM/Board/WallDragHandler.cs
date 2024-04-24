using UnityEngine;

namespace blockade.Blockade_IHM
{
    public class WallDragHandler : MonoBehaviour
    {
        private GameObject selectedObject;

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Ex�cut�e toutes les frames
        /// </summary>
        void Update()
        {
            // D�place le mur avec le curseur tant qu'il est s�lectionn�
            if (selectedObject != null)
            {
                Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
                selectedObject.transform.position = new Vector3(worldPosition.x, 1.25f, worldPosition.z);

                // Garder la possibilit� de faire tourner un mur ? (il faut changer l'attribut isVerti si c'est le cas, et supprimer le bon mur de l'espace de droite apr�s cela).
                /*
                if (Input.GetMouseButtonDown(1))
                {
                    selectedObject.transform.rotation = Quaternion.Euler(new Vector3(
                        selectedObject.transform.rotation.eulerAngles.x,
                        selectedObject.transform.rotation.eulerAngles.y + 90f,
                        selectedObject.transform.rotation.eulerAngles.z));
                }
                */
            }
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Impl�mentation de la fonction OnMouseDown()
        /// S�lectionne le mur � d�plac� si ce n'est pas d�j� fait ou envoie un DTO pour essayer de le placer sinon.
        /// 
        /// Publique
        /// </summary>
        public void OnMouseDown()
        {
            // if no walls are selected
            if (selectedObject == null)
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
                    if (!ihm.GetPlayer(ihm.GetCurrentPlayer()).isPlacingWall || hit.collider.gameObject.GetComponent<Wall>().GetId() != ihm.GetCurrentPlayer())
                    {
                        return;
                    }

                    // select the object and make it transparent
                    selectedObject = hit.collider.gameObject;
                    selectedObject.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
                    Cursor.visible = false;
                }
            }
            // Send the dto to try to place the object
            else
            {
                // Get worldPosition using the position of the mouse
                Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
                //selectedObject.transform.position = new Vector3(worldPosition.x, 1f, worldPosition.z);

                // Rend l'apparence par d�faut � l'objet (POUT L'INSTANT SEULEMENT LA COULEUR DE BASE --> Cr�er une fonction resetColor dans Wall.cs)
                selectedObject.GetComponent<MeshRenderer>().material.color = Resources.Load<Material>("WallMaterial").color;

                // Affichage de la position du mur puis appel de la fonction d'envoie de DTO
                Debug.Log("WorldPosition : " + worldPosition);
                // FAIRE LA PARTIE EN CAS DE PLACEMENT REFUSE POUR GARDER LE MUR
                selectedObject.GetComponent<Wall>().sendDTOWall(worldPosition);
                Destroy(selectedObject);

                // D�selectionne l'objet et fait r�apparaitre le curseur
                selectedObject = null;
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
