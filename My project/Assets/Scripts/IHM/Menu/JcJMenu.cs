using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JcJMenu : MonoBehaviour
{
    public string Player1Name;
    public string Player2Name;

    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SetPlayer1Name (string inputName)
    {
        Player1Name = inputName;
        Debug.Log(Player1Name);
    }

    public void SetPlayer2Name (string inputName)
    {
        Player2Name = inputName;
        Debug.Log(Player2Name);
    }
}
