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

    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SetCreatorPlayerName (string inputName)
    {
        CreatorPlayerName = inputName;
        Debug.Log(CreatorPlayerName);
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
