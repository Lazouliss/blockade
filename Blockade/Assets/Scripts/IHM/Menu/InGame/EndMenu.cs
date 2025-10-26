using UnityEngine;

namespace blockade.Blockade_IHM
{
    public class EndMenu : MonoBehaviour
    {
        //public static bool GameIsWin = false;

        public GameObject medaille_victoire;
        public GameObject medaille_defaite;

        public string TypePartie;
        public IHM IHM;
        

        // Update is called once per frame
        /// <summary>
        /// Par Nolan Laroche
        /// 
        /// Fonction qui permet à l'affichage de  choisir la bonne medaille en fonction du résultat de la partie
        /// </summary>
        public void SelectWinner(bool gameIsWin)
        {
            if (gameIsWin)
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

        // Update is called once per frame
        /// <summary>
        /// Par Nolan Laroche
        /// 
        /// Fonction qui permet de passer les variables de parties actuelles et qui les renvoies 
        /// </summary>
        public void Replay()
        {
            TypePartie = IHM.GetTypePartie();
            Debug.Log(TypePartie);
            UIManager.SetTypePartie(TypePartie);
            
        }


    }
}