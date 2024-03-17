using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JcEMenu : MonoBehaviour
{
    public string PlayerName;
    public int levelValue;
    public string levelString;

    // Méthode qui change de scène (en l'occurence la scène de jeu)
    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // TODO envoi des variables a la scène d'apres (nom de joueur, type de partie etc...)
    }

    // Méthode qui set le nom du joueur
    public void SetPlayerName (string inputName)
    {
        try
        {
            if (inputName.Length > 15)
            {
                throw new Exception("Le nom du joueur ne doit pas dépasser 15 caractères.");
            }

            if (!IsAlphaNumeric(inputName))
            {
                throw new Exception("Le nom du joueur ne doit pas contenir de caractères spéciaux.");
            }

            PlayerName = inputName;
            Debug.Log(PlayerName);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Erreur : " + e.Message);
        }
    }

    // Méthode pour vérifier si une chaîne est composée uniquement de caractères alphanumériques
    private bool IsAlphaNumeric(string str)
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

    // Méthode qui set le niveau de l'IA adverse
    public void setIALevel (int level)
    {
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
