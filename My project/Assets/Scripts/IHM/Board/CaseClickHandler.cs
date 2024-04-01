using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CaseClickHandler : MonoBehaviour
{
    public Plateau plateau;
    private Renderer caseRenderer;
    private  static CaseClickHandler lastCaseClick; //en mode static pour avoir la dernière référence
    void Start()
    {
        caseRenderer = GetComponent<Renderer>();
    }

    private void OnMouseDown()
    {
        Debug.Log("Case cliquée " + name);
         if (lastCaseClick != null && lastCaseClick != this)
        {
            lastCaseClick.RestoreOriginalColor();
        }

        caseRenderer.material.color = Color.green;
        lastCaseClick = this;

    }

    //restauration des couleurs des cases du départ
    //lorsqu'on clique sur une autre case ça permettra de supp
    //la couleur de la case d'auparavant
     public void RestoreOriginalColor()
    {
        CaseClickHandler[] allCases = GetComponentsInChildren<CaseClickHandler>();

        foreach (CaseClickHandler caseObj in allCases)
        {
            Renderer caseRenderer = caseObj.gameObject.GetComponent<Renderer>();
            if (caseRenderer != null)
            {

                caseRenderer.material.color = Color.black;
            }
        }
    }
  
}