using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace blockade.Blockade_IHM
{
    public class GameResume : MonoBehaviour
    {
        public TMP_Text ResultText;
        public TMP_Text PlayerText;
        public TMP_Text ScoreText;
        public TMP_Text DateText;

        /// <summary>
        /// Par Martin GADET
        /// Méthode Setup qui set le text des TMP du prefab GameResume
        /// Publique
        /// </summary>
        /// <returns></returns>
        public void Setup(string result, string player, string score, string date)
        {
            ResultText.text = result;
            PlayerText.text = "contre : " + player;
            ScoreText.text = "en : " + score + " coups";
            DateText.text = date;

            if (result == "Victoire")
            {
                ResultText.color = Color.green;
            }
            else
            {
                ResultText.color = Color.red;
            }
        }
    }
}