using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Overlay : MonoBehaviour
{
    public GameObject remainingWalls;

    // temporary variables
    private int vertical_walls_left;
    private int horizontal_walls_left;

    public TextMeshProUGUI vertical;
    public TextMeshProUGUI horizontal;

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

        UpdateRemainingWalls("Vertical", 5);
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
}
