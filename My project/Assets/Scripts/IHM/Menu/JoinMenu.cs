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

    // Start is called before the first frame update
    void Start()
    {
        points = "";
        PointText.SetText(points);
        i = 0;
        wait = false;
    }

    // Update is called once per frame
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

    
    // Méthode qui change de scène (en l'occurence la scène de jeu)
    public void JoinGame ()
    {
        // TODO verifier si le code rentré est le meme que celui de la partie
        // si oui
        // Passer a un ecran d'attente
        menuObject.SetActive(false);
        waitScreenObject.SetActive(true);
        wait = true;
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
