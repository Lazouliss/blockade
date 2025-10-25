using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDev : MonoBehaviour
{
    public static bool DevOn = false;

    public GameObject DevMenuUI;
    [SerializeField] private KeyCode key_switchDevMode;

    /// <summary>
    /// Par Nolan Laroche
    /// 
    /// Fonction qui permet d'écouter pour savoir si la touche pour afficher le menu a été appuyé
    /// </summary>
    void Update()
{
    if (Input.GetKeyDown(key_switchDevMode))
    {
        // Si le menu dev est actuellement désactivé, l'activer
        if (!DevMenuUI.activeSelf)
        {
            Debug.Log("Affichage du menu dev");
            Pause();
        }
        // Sinon, s'il est actuellement activé, le désactiver
        else
        {
            Debug.Log("Masquage du menu dev");
            Resume();
        }
    }
}
/// <summary>
    /// Par Nolan Laroche
    /// 
    // Met en pause le jeu et affiche le menu dev
    /// </summary>
void Pause()
{
    DevMenuUI.SetActive(true);
    Time.timeScale = 0f;
}
 


    /// <summary>
    /// Par Nolan Laroche
    /// 
    // Reprend le jeu et cache le menu dev
    /// </summary>
public void Resume()
{
    DevMenuUI.SetActive(false);
    Time.timeScale = 1f;
}
   
   

}
