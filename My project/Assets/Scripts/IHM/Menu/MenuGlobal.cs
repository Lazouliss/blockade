using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading;

public class MenuGlobal : MonoBehaviour
{
    public static string typePartie; // ONLINE, JCJ, JCE, ECE

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui passe a la scène de jeu
    /// Publique
    /// </summary>
    /// <returns></returns>
    public static void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui set le type de la partie
    /// Publique
    /// </summary>
    /// <returns></returns>
    public static void SetTypePartie (string type)
    {
        typePartie = type;
        Debug.Log(typePartie);
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode pour vérifier si une chaîne est composée uniquement de caractères alphanumériques
    /// Publique
    /// </summary>
    /// <returns>Boolean (true/false)</returns>
    public static bool IsAlphaNumeric(string str)
    {
        foreach (char c in str)
        {
            if (!char.IsLetter(c) || char.IsWhiteSpace(c) || char.IsPunctuation(c) || char.IsSymbol(c))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui set le nom d'un joueur
    /// Publique
    /// </summary>
    /// <returns></returns>
    public static void SetPlayerName (string name, string inputName)
    {
        try
        {
            if (inputName.Length > 15)
            {
                throw new Exception("Le nom du joueur ne doit pas dépasser 15 caractères.");
            }

            if (!MenuGlobal.IsAlphaNumeric(inputName))
            {
                throw new Exception("Le nom du joueur ne doit pas contenir de caractères spéciaux.");
            }

            name = inputName;
            Debug.Log(name);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Erreur : " + e.Message);
        }
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui set le code rentré par un joueur
    /// Publique
    /// </summary>
    /// <returns></returns>
    public static void SetCode (int code, string inputCode)
    {
        try 
        {
            code = Int32.Parse(inputCode);
        }
        catch (FormatException)
        {
            Debug.Log("Put a integer number as code");
        }
        Debug.Log(code);
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui set le niveau de l'IA
    /// Publique
    /// </summary>
    /// <returns></returns>
    public static void setIALevel (int levelValue, string levelString, int level)
    {
        levelValue = 1;
        levelString = "Facile";
        switch(level)
        {
            case 0:
                levelValue = 1;
                levelString = "Facile";
                break;
            case 1:
                levelValue = 2;
                levelString = "Moyen";
                break;
            case 2:
                levelValue = 3;
                levelString = "Difficile";
                break;
        }

        Debug.Log(levelValue);
        Debug.Log(levelString);
    }
}
