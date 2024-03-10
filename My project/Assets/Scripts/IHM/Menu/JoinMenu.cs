using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JoinMenu : MonoBehaviour
{

    public string JoinerPlayerName;
    public int JoinerCode;

    // Méthode qui change de scène (en l'occurence la scène de jeu)
    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // TODO verifier si le code rentré est le meme que celui de la partie
        // TODO envoi des variables a la scène d'apres (nom de joueur, type de partie etc...)
    }

    // Méthode qui set le nom du joueur qui rejoint
    public void SetJoinerPlayerName(string inputName)
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

            JoinerPlayerName = inputName;
            Debug.Log(JoinerPlayerName);
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

    public void SetJoinerCode (string inputCode)
    {
        try 
        {
            JoinerCode = Int32.Parse(inputCode);
        }
        catch (FormatException)
        {
            Debug.Log("Put a integer number as code");
        }
        Debug.Log(JoinerCode);
    }
}
