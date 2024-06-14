using UnityEngine;

namespace blockade.Blockade_IHM
{
    //ABERKANE Doha
    public class PawnClickHandler : MonoBehaviour
    {
        public Board plateau;

        
        /// <summary>
        /// Par  ABERKANE Doha &  (Thomas Montigny partie où y a écrit IHM)
        /// 
        /// Deplacement du contenu de la fonction OnMouseDown() dans une ActionPawn pour qu'elle 
        /// puisse etre appelee par une autre partie (pour par exemple jouer un coup par une IA)
        /// </summary>
        public void OnMouseDown()
        {
            ActionPawn();
        }
        
        public void ActionPawn()
        {
            IHM ihm = this.plateau.GetComponent<Board>().ihm.GetComponent<IHM>();

            if (this.GetComponent<Pawn>().GetID() == ihm.GetCurrentPlayer() && !ihm.GetPlayer(ihm.GetCurrentPlayer()).isPlacingWall)
            {
                //afficher position
                Debug.Log("Position du pion cliqué  " + transform.position);

                //appel de la fonction afficherCoups possibles 
                plateau.afficherCoupsPossibles((transform.position.x, transform.position.z));

                //créer startPos
                Vector2 startPos = new Vector2(transform.position.x, transform.position.z);

                //envoi du dto 
                plateau.SendDTO(startPos, true);
            }
        }
    }
}