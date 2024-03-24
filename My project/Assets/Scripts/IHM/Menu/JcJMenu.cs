using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JcJMenu : MonoBehaviour
{
    public static string Player1Name;
    public static string Player2Name;

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui set le nom du joueur 1
    /// Publique
    /// </summary>
    /// <returns></returns>
    public void SetPlayer1Name (string inputName)
    {   
        MenuGlobal.SetPlayerName(Player1Name, inputName);
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui set le nom du joueur 2
    /// Publique
    /// </summary>
    /// <returns></returns>
    public void SetPlayer2Name (string inputName)
    {
        MenuGlobal.SetPlayerName(Player2Name, inputName);
    }
}
