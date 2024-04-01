using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnClickHandler : MonoBehaviour
{
    private void OnMouseDown()
    {
        // Mettez ici la logique à exécuter lorsque le pion est cliqué
        Debug.Log("Pion cliqué : " + gameObject.name);
    }
}
