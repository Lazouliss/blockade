using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Threading;

public class JoinMenu : MonoBehaviour
{

    public string JoinerPlayerName;
    public int JoinerCode;
    public string points;
    public TMP_Text PointText;
    public int i;
    public GameObject menuObject;
    public GameObject waitScreenObject;
    public bool wait;

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui set les variables de départ pour l'écran d'attente
    /// Publique
    /// </summary>
    /// <returns></returns>
    void Start()
    {
        points = "";
        PointText.SetText(points);
        i = 0;
        wait = false;
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui se lance toute les seconde pour azfficher l'écran d'attente
    /// Publique
    /// </summary>
    /// <returns></returns>
    void Update()
    {
        // quand partie commence
        // TODO envoi des variables a la scène d'apres (nom de joueur, type de partie etc...)
        if(wait == true)
        {
            points += ".";
            PointText.SetText(points);
            i += 1;
            if(i == 3)
            {
                points = "";
                i = 0;
            }
            Thread.Sleep(500);
        }
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui change de scène (en l'occurence la scène de jeu)
    /// Publique
    /// </summary>
    /// <returns></returns>
    public void JoinGame ()
    {
        // TODO verifier si le code rentré est le meme que celui de la partie
        // si oui
        // Passer a un ecran d'attente
        menuObject.SetActive(false);
        waitScreenObject.SetActive(true);
        wait = true;
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui set le nom du joueur qui rejoint
    /// Publique
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Par Martin GADET
    /// Méthode Méthode qui set le code rentré par le joueur qui rejoint
    /// Publique
    /// </summary>
    /// <returns></returns>
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
