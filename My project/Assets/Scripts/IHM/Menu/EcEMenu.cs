using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EcEMenu : MonoBehaviour
{
    public int levelValue1;
    public string levelString1;
    public int levelValue2;
    public string levelString2;

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
    /// Méthode qui set le niveau de l'IA 1
    /// Publique
    /// </summary>
    /// <returns></returns>
    public void setIALevel1 (int level)
    {
        switch(level)
        {
            case 0:
                levelValue1 = 1;
                levelString1 = "Facile";
                break;
            case 1:
                levelValue1 = 2;
                levelString1 = "Moyen";
                break;
            case 2:
                levelValue1 = 3;
                levelString1 = "Difficile";
                break;
        }

        Debug.Log(levelValue1);
        Debug.Log(levelString1);
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui set le niveau de l'IA 2
    /// Publique
    /// </summary>
    /// <returns></returns>
    public void setIALevel2 (int level)
    {
        switch(level)
        {
            case 0:
                levelValue2 = 1;
                levelString2 = "Facile";
                break;
            case 1:
                levelValue2 = 2;
                levelString2 = "Moyen";
                break;
            case 2:
                levelValue2 = 3;
                levelString2 = "Difficile";
                break;
        }

        Debug.Log(levelValue2);
        Debug.Log(levelString2);
    }
}
