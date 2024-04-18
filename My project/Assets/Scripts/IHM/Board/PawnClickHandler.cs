using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using blockade.Blockade_common;

//ABERKANE Doha
public class PawnClickHandler : MonoBehaviour
{
    public Plateau plateau;
    public Common.DTOPawn dto;
    
    //ABERKANE Doha
     public void Start()
    {
        //creation du dto
        dto = new Common.DTOPawn();

    }

    //ABERKANE Doha
    public void OnMouseDown()
    {
        //afficher position
        Debug.Log("Position du pion cliqué  " + transform.position);

        //créer startPos
        dto.startPos = (transform.position.x, transform.position.z);
        
        //envoi du dto 
        plateau.SendDTO(dto);

    }

}

