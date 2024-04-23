using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using blockade.Blockade_common;

//ABERKANE Doha
public class PawnClickHandler : MonoBehaviour
{
    public Plateau plateau;

    //ABERKANE Doha
    public void OnMouseDown()
    {
        IHM ihm = this.plateau.GetComponent<Plateau>().ihm.GetComponent<IHM>();

        if (this.GetComponent<Pawn>().GetID() == ihm.GetCurrentPlayer() && !ihm.GetPlayer(ihm.GetCurrentPlayer()).isPlacingWall)
        {
            //afficher position
            Debug.Log("Position du pion cliqué  " + transform.position);

            //créer startPos
            Vector2 startPos = new Vector2(transform.position.x, transform.position.z);

            // sélection du pion
            plateau.SelectPawn(this.GetComponent<Pawn>());

            //envoi du dto 
            plateau.SendDTO(startPos, true);
        }
    }

}