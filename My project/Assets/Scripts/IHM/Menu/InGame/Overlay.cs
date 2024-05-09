using UnityEngine;
using TMPro;

namespace blockade.Blockade_IHM
{
    public class Overlay : MonoBehaviour
    {
        // Overlay pour les murs restants
        [SerializeField] private GameObject remainingWalls;
        [SerializeField] private TextMeshProUGUI vertical;
        [SerializeField] private TextMeshProUGUI horizontal;

        // Cameras
        [SerializeField] private Camera playerCam;
        [SerializeField] private Camera boardCam;
        [SerializeField] private GameObject btn_switchCamera;
        [SerializeField] private KeyCode key_switchCamera;

        // Overlay pour l'action a realiser
        [SerializeField] private TextMeshProUGUI txtMovePawn;
        [SerializeField] private TextMeshProUGUI txtPlaceWall;
        [SerializeField] private TextMeshProUGUI txtError;

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Initialise les cameras.
        /// </summary>
        void Start()
        {
            // Initialise les cameras
            playerCam.enabled = true;
            boardCam.enabled = false;

            // Initialise les actions du joueur
            txtMovePawn.enabled = true;
            txtPlaceWall.enabled = false;
            txtError.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(this.key_switchCamera))
            {
                Debug.Log("Switched camera with input key");
                this.SwitchCamera();
            }
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        ///
        /// Renvoie l'etat de la camera joueur (pour permettre a l'ihm de tester la condition de fin)
        ///
        /// Publique
        /// </summary>
        /// <returns></returns>
        public bool GetPlayerCamState()
        {
            return playerCam.enabled;
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Permet de mettre à jour le nombre de murs restants sur l'overlay
        /// Publique
        /// </summary>
        /// <param name="type">'Vertical' ou 'Horizontal'</param>
        /// <param name="value">Nombre de murs restants</param>
        public void UpdateRemainingWalls(string type, int value)
        {
            if (type == "Vertical")
            {
                vertical.text = "Verticaux : " + value;
            }
            else if (type == "Horizontal")
            {
                horizontal.text = "Horizontaux : " + value;
            }
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Permet de passer d'une caméra à une autre en un clique
        /// Change aussi l'état de l'écouteur d'audio (pour n'en avoir qu'un seul sur la scène)
        /// Publique
        /// </summary>
        public void SwitchCamera()
        {
            Debug.Log("Changing Camera");
            playerCam.enabled = !playerCam.enabled;
            playerCam.GetComponent<AudioListener>().enabled = !playerCam.GetComponent<AudioListener>().enabled;

            boardCam.enabled = !boardCam.enabled;
            boardCam.GetComponent<AudioListener>().enabled = !boardCam.GetComponent<AudioListener>().enabled;

            if (playerCam.enabled)
            {
                playerCam.tag = "MainCamera";
                boardCam.tag = "Untagged";
            }
            else
            {
                playerCam.tag = "Untagged";
                boardCam.tag = "MainCamera";
            }
            Debug.Log("PlayerCam Tag : " + playerCam.tag + ", BoardCam Tag : " + boardCam.tag);
        }

        public void SwitchActionPlayer()
        {
            txtMovePawn.enabled = !txtMovePawn.enabled;
            txtPlaceWall.enabled = !txtPlaceWall.enabled;
        }

        public void ToggleError(bool state)
        {
            txtError.enabled = state;
        }
    }
}