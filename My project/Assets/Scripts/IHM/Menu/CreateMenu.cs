using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CreateMenu : MonoBehaviour
{
    public static string CreatorPlayerName;

    public int CreatorCode;

    public int nbPlayer;
    public TMP_Text CodeText;
    public TMP_Text NumberText;

    /// <summary>
    /// Par Martin GADET
    /// Méthode Start qui récupère le code de la partie et l'affiche
    /// Publique
    /// </summary>
    /// <returns></returns>
    void Start()
    {
        string testcode = "1234";
        DisplayCreatorCode(testcode);
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode Update qui récupère le nombre de joueur chaque seconde et l'affiche
    /// Publique
    /// </summary>
    /// <returns></returns>
    void Update()
    {
        DisplayNumberOfPlayer(1);
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui set le nom du joueur qui crée la partie
    /// Publique
    /// </summary>
    /// <returns></returns>
    public void SetCreatorPlayerName (string inputName)
    {
        MenuGlobal.SetPlayerName(CreatorPlayerName, inputName);
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui affiche le code de la partie
    /// Publique
    /// </summary>
    /// <returns></returns>
    public void DisplayCreatorCode (string code)
    {
        MenuGlobal.SetCode(CreatorCode, code);
        CodeText.SetText(CreatorCode.ToString());
        Debug.Log(CreatorCode);
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui affiche le nombre de joueur dans la partie
    /// Publique
    /// </summary>
    /// <returns></returns>
    public void DisplayNumberOfPlayer (int nb)
    {
        nbPlayer = nb;
        NumberText.SetText(nbPlayer.ToString());
        Debug.Log(nbPlayer);
        // TODO comment recup l'info ?
    }
}
