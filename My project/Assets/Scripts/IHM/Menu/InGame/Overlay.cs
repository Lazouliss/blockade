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
        [SerializeField] private Camera player2Cam;         // NORMALEMENT INUTILE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        [SerializeField] private Camera board2Cam;          // NORMALEMENT INUTILE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        [SerializeField] private GameObject btn_switchCamera;
        [SerializeField] private KeyCode key_switchCamera;

        private IHM ihm;

        // tests not working
        /*
        private GameObject verti;
        private GameObject horiz;
        */

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Initialise la page, place automatiquement les éléments.
        /// </summary>
        void Start()
        {
            // Move the remaining walls labels to the top left corner
            //remainingWalls.transform.position = new Vector3(110, this.GetComponent<RectTransform>().rect.height - 60, 0);

            // for tests
            //UpdateRemainingWalls("Vertical", 5);
            //UpdateRemainingWalls("Horizontal", 8);

            // tests not working
            /*
            verti = GameObject.Find("RemainingWalls_Vertical");
            horiz = GameObject.Find("RemainingWalls_Horizontal");
            //Debug.Log(remainingWalls.transform.Find("RemainingWalls_Title").position);
            Debug.Log(verti.GetComponent<Transform>().position);
            //Debug.Log(verti.GetComponent<>);
            Debug.Log(horiz.transform.position);
            */

            // Recupere l'ihm --> NORMALEMENT INUTILE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            ihm = GameObject.Find("Game").GetComponent<IHM>();

            // Initialise les cameras
            playerCam.enabled = true;
            boardCam.enabled = false;
            player2Cam.enabled = true;      // NORMALEMENT INUTILE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            board2Cam.enabled = false;      // NORMALEMENT INUTILE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

            // Move the switch camera button to the top right corner
            //btn_switchCamera.transform.position = new Vector3(this.GetComponent<RectTransform>().rect.width - 32, this.GetComponent<RectTransform>().rect.height - 32, 0);
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
            // RE-RENDRE GENERIQUE !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            Debug.Log("Changing Camera");
            if (ihm.GetCurrentPlayer() == 1)
            {
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
            else
            {
                player2Cam.enabled = !player2Cam.enabled;
                player2Cam.GetComponent<AudioListener>().enabled = !player2Cam.GetComponent<AudioListener>().enabled;

                board2Cam.enabled = !board2Cam.enabled;
                board2Cam.GetComponent<AudioListener>().enabled = !board2Cam.GetComponent<AudioListener>().enabled;

                if (player2Cam.enabled)
                {
                    player2Cam.tag = "MainCamera";
                    board2Cam.tag = "Untagged";
                }
                else
                {
                    player2Cam.tag = "Untagged";
                    board2Cam.tag = "MainCamera";
                }
                Debug.Log("Player2Cam Tag : " + player2Cam.tag + ", Board2Cam Tag : " + board2Cam.tag);
            }

        }
    }
}