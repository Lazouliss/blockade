using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameList : MonoBehaviour
{
    public GameObject gameResumeTemplate;
    public Transform Content;

    /// <summary>
    /// Par Martin GADET
    /// Méthode Start pour les test
    /// Publique
    /// </summary>
    /// <returns></returns>
    void Start()
    {
        addGameResume("Victoire", "Player1", "100", "19/04/2024");
        addGameResume("Defaite", "Player5", "89", "19/04/2024");
        addGameResume("Defaite", "Player5", "87", "19/04/2024");
        addGameResume("Victoire", "Bot3", "105", "19/04/2024");
        addGameResume("Victoire", "Bot2", "57", "19/04/2024");
        addGameResume("Defaite", "Marty", "90", "19/04/2024");
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode addGameResume qui permet d'ajouter un résumùé de partie au menu
    /// Publique
    /// </summary>
    /// <returns></returns>
    public void addGameResume(string result, string player, string score, string date)
    {
        GameObject gameResumeInstance = Instantiate(gameResumeTemplate, Content);
        GameResume gameResume = gameResumeInstance.GetComponent<GameResume>();
        gameResume.Setup(result, player, score, date);
    }
}
