using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace blockade.Blockade_IHM
{
    public class MainMenu : MonoBehaviour
    {
        /// <summary>
        /// Par Martin GADET
        /// Méthode qui quitte le jeu
        /// Publique
        /// </summary>
        /// <returns></returns>
        public void QuitGame()
        {
            Debug.Log("QUIT!");
            Application.Quit();
        }


    }
}