using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace blockade.Blockade_IHM
{
    public class EndMenu : MonoBehaviour
    {
        public static bool GameIsWin = false;

        public GameObject medaille_victoire;
        public GameObject medaille_defaite;

        // Update is called once per frame
        /// <summary>
        /// Par Nolan Laroche
        /// 
        /// Fonction qui permet à l'affichage de  choisir la bonne medaille en fonction du résultat de la partie
        /// </summary>
        void Start()
        {
            if (GameIsWin)
            {
                medaille_defaite.gameObject.SetActive(false);
                medaille_victoire.gameObject.SetActive(true);
            }
            else
            {
                medaille_victoire.gameObject.SetActive(false);
                medaille_defaite.gameObject.SetActive(true);
            }
        }





    }
}