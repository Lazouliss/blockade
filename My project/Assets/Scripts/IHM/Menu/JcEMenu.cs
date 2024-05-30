using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JcEMenu : MonoBehaviour
{
    public string PlayerName;
    public int levelValue;
    public string levelString;

    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SetPlayerName (string inputName)
    {
        PlayerName = inputName;
        Debug.Log(PlayerName);
    }

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
