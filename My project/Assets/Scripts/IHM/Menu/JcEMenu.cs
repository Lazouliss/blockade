using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class JcEMenu : MonoBehaviour
{
    public static string PlayerName;
    public static int levelValue;
    public static string levelString;

    public TMP_Text PlayerNameText;

    /// <summary>
    /// Par Martin GADET
    /// Méthode Start qui récupère le nom du joueur et l'affiche
    /// Publique
    /// </summary>
    /// <returns></returns>
    void Start()
    {
        DisplayPlayerName(ConnnectionMenu.PlayerName);
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
    /// Méthode qui set le niveau de l'IA adverse
    /// Publique
    /// </summary>
    /// <returns></returns>
    public void setIALevel (int level)
    {
        levelValue = MenuGlobal.setIALevel(level).Item1;
        levelString = MenuGlobal.setIALevel(level).Item2;
        Debug.Log("levelValue : " + levelValue);
        Debug.Log("levelString : " + levelString);
    }
}
