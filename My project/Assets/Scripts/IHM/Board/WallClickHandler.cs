using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClickHandler : MonoBehaviour
{

    public Plateau plateau;
    private Renderer wallRenderer;
    private  static WallClickHandler lastClick; //en mode static pour avoir la dernière référence
    void Start()
    {
        wallRenderer = GetComponent<Renderer>();
    }

    private void OnMouseDown()
    {
        Debug.Log("Mur cliqué: " + name);
         if (lastClick != null && lastClick != this)
        {
            lastClick.RestoreOriginalColor();
        }

        wallRenderer.material.color = Color.magenta;
        lastClick = this;


    }

    private void RestoreOriginalColor()
    {

        if (wallRenderer != null)
        {
            wallRenderer.material.color = Color.white;

        }
    }
}

