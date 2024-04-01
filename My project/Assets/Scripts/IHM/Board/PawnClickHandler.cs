using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ABERKANE Doha
public class PawnClickHandler : MonoBehaviour
{


    private void OnMouseDown()
    {
        //afficher position
        Debug.Log("Pion cliqué " + transform.position);
        
    }

}
