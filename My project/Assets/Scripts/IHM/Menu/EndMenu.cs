using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMenu : MonoBehaviour
{
    public static bool GameIsEnded = false;

    public GameObject pauseMenuUI;
    

    // Update is called once per frame
    void Update()
    {
       if(GameIsEnded)
       {
            Pause();
       }
       else
       {
            Resume();
       }
        
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsEnded = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsEnded = true;
    }

    public void ReplayMenu()
    {
        Debug.Log("Replay ...");
        //SceneManager.LoadScene("Menu")
    }

    public void StartMenu()
    {
        Debug.Log("Returning to the Start Menu...");
        
    }
}
