using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameList : MonoBehaviour
{
    public GameObject gameResumeTemplate;
    public Transform Content;

    // Start is called before the first frame update
    void Start()
    {
        addGameResume("Victoire", "Player1", "100", "2024-04-19");
        addGameResume("Defaite", "Player5", "89", "2024-04-19");
        addGameResume("Defaite", "Player5", "87", "2024-04-19");
        addGameResume("Victoire", "Bot3", "105", "2024-04-19");
        addGameResume("Victoire", "Bot2", "57", "2024-04-19");
        addGameResume("Defaite", "Marty", "90", "2024-04-19");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addGameResume(string result, string player, string score, string date)
    {
        ShiftGameResumes();
        GameObject gameResumeInstance = Instantiate(gameResumeTemplate, Content);
        GameResume gameResume = gameResumeInstance.GetComponent<GameResume>();
        gameResume.Setup(result, player, score, date);
    }

    public void ShiftGameResumes()
    {
        int numGameResumes = Content.childCount;
        for (int i = numGameResumes - 1; i >= 0; i--)
        {
            Transform gameResumeTransform = Content.GetChild(i);
            Vector3 newPosition = gameResumeTransform.localPosition;
            newPosition.y -= 130f;
            gameResumeTransform.localPosition = newPosition;
        }
    }
}
