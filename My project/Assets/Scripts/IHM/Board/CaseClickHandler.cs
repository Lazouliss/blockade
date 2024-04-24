using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using blockade.Blockade_common;

namespace blockade.Blockade_IHM
{
    public class CaseClickHandler : MonoBehaviour
    {
        public Plateau plateau;
        public Renderer caseRenderer;
        public static CaseClickHandler lastCaseClick; //en mode static pour avoir la dernière référence

        //ABERKANE Doha
        void Start()
        {
            caseRenderer = GetComponent<Renderer>();
        }

        //ABERKANE Doha
        public void OnMouseDown()
        {
            Debug.Log("Case cliquée " + name);

            IHM ihm = this.plateau.GetComponent<Plateau>().ihm.GetComponent<IHM>();

            if (!ihm.GetPlayer(ihm.GetCurrentPlayer()).isPlacingWall)
            {
                //check si la case n est pas null et si la case n'est pas égale à elle même 
                //alors on restaure la couleur original de la case
                if (lastCaseClick != null && lastCaseClick != this)
                {
                    lastCaseClick.RestoreOriginalColor();
                }
                //couleur de la case cliquée
                caseRenderer.material.color = Color.green;
                //enregistrer la valeur de la case
                lastCaseClick = this;

                //valeur destPos ajouté au dto 
                Vector2 destPos = new Vector2(transform.position.x, transform.position.z);

                //envoi du dto 
                plateau.GetComponent<Plateau>().SendDTO(destPos, false);
            }
        }
        //ABERKANE Doha
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
}