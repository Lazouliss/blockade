using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CreateMenu : MonoBehaviour
{
    public string CreatorPlayerName;
    public int CreatorCode;
    public int nbPlayer;
    public TMP_Text CodeText;
    public TMP_Text NumberText;

    void Start()
    {
        SetCreatorCode (1234);
        SetNumberOfPlayer (1);
    }

    void Update()
    {
        SetNumberOfPlayer (1);
    }

    // Méthode qui change de scène (en l'occurence la scène de jeu)
    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // TODO envoi des variables a la scène d'apres (nom de joueur, type de partie etc...)
        // TODO envoi l'info que la partie commence a lautre joueur
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

    // Méthode qui affiche le code de la partie 
    public void SetCreatorCode (int code)
    {
        CreatorCode = code;
        CodeText.SetText(CreatorCode.ToString());
        Debug.Log(CreatorCode);
    }

    // Méthode qui affiche le nombre de joueur dans la partie
    public void SetNumberOfPlayer (int nb)
    {
        nbPlayer = nb;
        NumberText.SetText(nbPlayer.ToString());
        Debug.Log(nbPlayer);
        // TODO comment recup l'info ?
    }
}
