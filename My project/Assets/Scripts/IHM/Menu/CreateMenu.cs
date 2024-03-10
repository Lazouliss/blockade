using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateMenu : MonoBehaviour
{
    public string CreatorPlayerName;
    public int CreatorCode;
    public int nbPlayer;
    public Text CodeText;
    public Text NumberText;

    // Méthode qui change de scène (en l'occurence la scène de jeu)
    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // TODO verifier si le code rentré est le meme que celui de la partie
        // TODO envoi des variables a la scène d'apres (nom de joueur, type de partie etc...)
    }

    // Méthode qui set le nom du joueur qui crée la partie
    public void SetCreatorPlayerName (string inputName)
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

            CreatorPlayerName = inputName;
            Debug.Log(CreatorPlayerName);
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

    public void SetCreatorCode (int code)
    {
        CreatorCode = code;
        // CodeText = GetComponent<Text>();
        CodeText.text = CreatorCode.ToString();
        Debug.Log(CreatorCode);
    }

    public void SetNumberOfPlayer (int nb)
    {
        nbPlayer = nb;
        // NumberText = GetComponent<Text>();
        NumberText.text = nbPlayer.ToString();
        Debug.Log(nbPlayer);
    }
}
