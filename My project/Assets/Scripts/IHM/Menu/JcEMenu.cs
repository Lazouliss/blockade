using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JcEMenu : MonoBehaviour
{
    public static string PlayerName;
    public static int levelValue;
    public static string levelString;

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui set le nom du joueur
    /// Publique
    /// </summary>
    /// <returns></returns>
    public void SetPlayerName (string inputName)
    {
        MenuGlobal.SetPlayerName(PlayerName, inputName);
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui set le niveau de l'IA adverse
    /// Publique
    /// </summary>
    /// <returns></returns>
    public void setIALevel (int level)
    {
        MenuGlobal.setIALevel(levelValue, levelString, level);
    }
}
