using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Méthode qui quitte le jeu
    public void QuitGame ()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
