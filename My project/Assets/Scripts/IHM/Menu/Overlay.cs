using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Overlay : MonoBehaviour
{
    // Overlay pour les murs restants
    public GameObject remainingWalls;
    public TextMeshProUGUI vertical;
    public TextMeshProUGUI horizontal;

    // temporary variables
    private int vertical_walls_left;
    private int horizontal_walls_left;

    // Cameras
    public Camera playerCam;
    public Camera boardCam;

    // tests not working
    /*
    private GameObject verti;
    private GameObject horiz;
    */

    /// <summary>
    /// Initialise la page, place automatiquement les éléments.
    /// 
    /// Par Thomas MONTIGNY
    /// </summary>
    void Start()
    {
        // for tests
        vertical_walls_left = 10;
        horizontal_walls_left = 8;

        // Change the remaining walls labels to the top left
        remainingWalls.transform.position = new Vector3(110, this.GetComponent<RectTransform>().rect.height - 60, 0);

        vertical.text = "Verticaux : " + vertical_walls_left;
        horizontal.text = "Horizontaux : " + horizontal_walls_left;

        // tests not working
        /*
        verti = GameObject.Find("RemainingWalls_Vertical");
        horiz = GameObject.Find("RemainingWalls_Horizontal");
        Debug.Log(remainingWalls.transform.Find("RemainingWalls_Title").position);
        Debug.Log(verti.transform.position);
        Debug.Log(horiz.transform.position);
        */

        // for tests
        UpdateRemainingWalls("Vertical", 5);

        // Inialise les cameras
        playerCam.enabled = true;
        boardCam.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Permet de mettre à jour le nombre de murs restants sur l'overlay
    ///
    /// Par Thomas MONTIGNY
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
    /// Permet de passer d'une caméra à une autre en un clique
    ///
    /// Par Thomas MONTIGNY
    /// Publique
    /// </summary>
    public void ClickSwitchCamera()
    {
        Debug.Log("Changed Camera");

        playerCam.enabled = !playerCam.enabled;
        boardCam.enabled = !boardCam.enabled;
    }
}