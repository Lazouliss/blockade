using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameResume : MonoBehaviour
{
    public TMP_Text ResultText;
    public TMP_Text PlayerText;
    public TMP_Text ScoreText;
    public TMP_Text DateText;

    public GameResume(string result, string player, string score, string date)
    {
        ResultText.text = result;
        PlayerText.text = player;
        ScoreText.text = score;
        DateText.text = date;
    }
}
