using UnityEngine;

namespace blockade.Blockade_IHM
{
    public class PauseMenu : MonoBehaviour
    {

        public static bool GameIsPaused = false;

        public GameObject pauseMenuUI;

        public GameObject btn_pause;

        // Update is called once per frame
        /// <summary>
        /// Par Nolan Laroche
        /// 
        /// Permet de gerer la situation du menu pause
        /// </summary>
        public void IsPause()
        {

            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }


        /// <summary>
        /// Par Nolan Laroche
        /// 
        /// Permet de retirer la pause de l'appli
        /// </summary>
        public void Resume()
        {
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            GameIsPaused = false;
        }

        /// <summary>
        /// Par Nolan Laroche
        /// 
        /// Permet de mettre en pause l'appli
        /// </summary>
        void Pause()
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            GameIsPaused = true;
        }

        /// <summary>
        /// Par Nolan Laroche
        /// 
        /// Permet de Quitter l'application depuis le menu Pause
        /// </summary>
        public void QuitGame()
        {
            Debug.Log("Quitting game...");
            Application.Quit();
        }
    }
}