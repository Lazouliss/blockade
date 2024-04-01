using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ABERKANE Doha
//MARCHE PAS pour le moment
public class PawnClickHandler : MonoBehaviour
{

    public Plateau plateau;
    private Renderer pawnRenderer;
    private static PawnClickHandler lastClickedPawn; 
    private Color originalColor; 

    private void Start()
    {
        originalColor = pawnRenderer.material.color;
    }

    private void OnMouseDown()
    {
        //si le pion est différent du dernier pion cliquéon restaure la couleur du dernier pion
        if (lastClickedPawn != null && lastClickedPawn != this)
        {
            lastClickedPawn.RestoreOriginalColor();
        }

        RestoreOriginalColor();

        //màj de la référence du dernier pion cliqué
        lastClickedPawn = this;
    }

    //fonction restauration couleur
    private void RestoreOriginalColor()
    {
        Renderer pawnRenderer = GetComponent<Renderer>();
        pawnRenderer.material.color = originalColor;
    }
}
