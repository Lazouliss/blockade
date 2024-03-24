using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JcJMenu : MonoBehaviour
{
    public string Player1Name;
    public string Player2Name;

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui change de scène (en l'occurence la scène de jeu)
    /// Publique
    /// </summary>
    /// <returns></returns>
    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // TODO envoi des variables a la scène d'apres (nom de joueur, type de partie etc...)
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui set le nom du joueur 1
    /// Publique
    /// </summary>
    /// <returns></returns>
    public void SetPlayer1Name (string inputName)
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

            Player1Name = inputName;
            Debug.Log(Player1Name);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Erreur : " + e.Message);
        }
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui set le nom du joueur 2
    /// Publique
    /// </summary>
    /// <returns></returns>
    public void SetPlayer2Name (string inputName)
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

            Player2Name = inputName;
            Debug.Log(Player2Name);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Erreur : " + e.Message);
        }
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode pour vérifier si une chaîne est composée uniquement de caractères alphanumériques
    /// Publique
    /// </summary>
    /// <returns></returns>
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
}
