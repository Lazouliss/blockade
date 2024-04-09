using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class JcJMenu : MonoBehaviour
{
    public static string Player2Name;

    public TMP_Text PlayerNameText;

    /// <summary>
    /// Par Martin GADET
    /// Méthode Start qui récupère le nom du joueur et l'affiche
    /// Publique
    /// </summary>
    /// <returns></returns>
    void Start()
    {
        DisplayPlayerName(ConnectionMenu.PlayerName);
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui affiche le nom du joueur
    /// Publique
    /// </summary>
    /// <returns></returns>
    public void DisplayPlayerName(string name)
    {
        PlayerNameText.SetText(name);
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui set le nom du joueur 2
    /// Publique
    /// </summary>
    /// <returns></returns>
    public void SetPlayer2Name (string inputName)
    {
        if(MenuGlobal.SetPlayerName(inputName) != "0")
        {
            Player2Name = MenuGlobal.SetPlayerName(inputName);
            Debug.Log("Player2 : " + Player2Name);
        }
    }
}
