using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading;

public class JoinMenu : MonoBehaviour
{

    public static string JoinerPlayerName;
    
    public int JoinerCode;

    public string points;
    public int i;
    public bool wait;
    public TMP_Text PointText;

    public GameObject menuObject;
    public GameObject waitScreenObject;

    /// <summary>
    /// Par Martin GADET
    /// Méthode Start qui set les valeur de départ pour l'écran d'attente
    /// Publique
    /// </summary>
    /// <returns></returns>
    void Start()
    {
        points = "";
        i = 0;
        wait = false;
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui se lance toute les secondes pour afficher l'écran d'attente
    /// Publique
    /// </summary>
    /// <returns></returns>
    void Update()
    {
        // TODO récupérer l'info de lancement de partie
        // si partie lancée alors appeler MenuGlobal.PlayGame()
        // sinon
        if(wait == true)
        {
            points += ".";
            PointText.SetText(points);
            i += 1;
            if(i == 3)
            {
                points = "";
                i = 0;
            }
            Thread.Sleep(500);
        }
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui affiche l'écran d'attente et met la variable wait a true
    /// Publique
    /// </summary>
    /// <returns></returns>
    public void JoinGame ()
    {
        // TODO verifier si le code rentré est le meme que celui de la partie
        // si oui
        // Passer a un ecran d'attente
        menuObject.SetActive(false);
        waitScreenObject.SetActive(true);
        wait = true;
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui set le nom du joueur qui rejoint
    /// Publique
    /// </summary>
    /// <returns></returns>
    public void SetJoinerPlayerName(string inputName)
    {
        MenuGlobal.SetPlayerName(JoinerPlayerName, inputName);
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui set le code rentré par le joueur qui rejoint
    /// Publique
    /// </summary>
    /// <returns></returns>
    public void SetJoinerCode (string inputCode)
    {
        MenuGlobal.SetCode(JoinerCode, inputCode);
    }
}
