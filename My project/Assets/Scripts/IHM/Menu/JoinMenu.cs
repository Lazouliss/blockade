using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JoinMenu : MonoBehaviour
{

    public string JoinerPlayerName;
    public int JoinerCode;

    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SetJoinerPlayerName (string inputName)
    {
        JoinerPlayerName = inputName;
        Debug.Log(JoinerPlayerName);
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
