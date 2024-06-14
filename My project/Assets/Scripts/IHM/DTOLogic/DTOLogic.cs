using blockade.Blockade_common;
using UnityEngine;

namespace blockade.Blockade_IHM
{
    /// <summary>
    /// Par Thomas MONTIGNY
    /// 
    /// Classe qui gere toutes la logique des DTOs de la partie IHM
    /// </summary>
    public class DTOLogic
    {
        private IHM ihm;
        private GameManager gamelogic;

        public DTOLogic(IHM ihm, GameManager gamelogic)
        {
            this.ihm = ihm;
            this.gamelogic = gamelogic;
        }

        public GameManager getGameLogic()
        {
            return gamelogic;
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Fonction pour recevoir les DTOs, appele par la logique de jeu
        /// Publique
        /// </summary>
        /// <param name="dto"></param>
        public void sendDTO(object dto)
        {
            applyDTO(dto);
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Pour appliquer un DTO
        /// Privee
        /// </summary>
        /// <param name="dto"></param>
        private void applyDTO(object dto)
        {
            switch (dto)
            {
                case Common.DTOWall dtoWall: applyDTOWall(dtoWall); break;

                case Common.DTOPawn dtoPawn: applyDTOPawn(dtoPawn); break;

                case Common.DTOGameState dtoGameState: applyDTOGameState(dtoGameState); break;

                case Common.DTOError dtoError: applyDTOError(dtoError); break;

                default: Debug.Log("error during applying dto"); break;
            }
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Applique un DTO mur
        /// Privee
        /// </summary>
        /// <param name="dto"></param>
        private void applyDTOWall(Common.DTOWall dto)
        {
            // Fait disparaitre le mur que le joueur avait de selectionne
            if (ihm.GetTypePartie() == "JCJ" || (ihm.GetTypePartie() == "JCE" && ihm.GetCurrentPlayer() == 1) || (ihm.GetTypePartie() == "Online" && ihm.GetCurrentPlayer() == 1))
            {
                Debug.Log("Forgetting wall");
                this.ihm.board.GetSelectedWall().GetComponent<WallDragHandler>().UnSelectWall();
            }

            // Puis applique le nouveau mur
            Debug.Log("applyDTOWall, coord1 = " + dto.coord1 + ", coord2 = " + dto.coord2 + ", direction = " + dto.direction + ", isAdd = " + dto.isAdd);
            ihm.gestionDTO.actionWall(dto, ihm.board);

            // Supprime le mur de la liste en fonction de sa direction
            if (dto.direction == Common.Direction.UP)
            {
                ihm.EditStackHorizontalWall(ihm.GetCurrentPlayer(), null);
            } else
            {
                ihm.EditStackVerticalWall(ihm.GetCurrentPlayer(), null);
            }

            // Le joueur viens de d�placer un mur donc sa prochaine action est de d�placer un pion
            ihm.SetPlayerPlacingWall(ihm.GetCurrentPlayer(), false);
            ihm.SwitchActionPlayer();
            ihm.ToggleError(false);
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Applique un DTO pion
        /// Privee
        /// </summary>
        /// <param name="dto"></param>
        private void applyDTOPawn(Common.DTOPawn dto)
        {
            Debug.Log("applyDTOPawn, startPos = " + dto.startPos + ", destPos = " + dto.destPos + ", mooves = " + dto.mooves);
            //Debug.Log("applyDTOPawn, startPos = " + dto.startPos + ", destPos = " + dto.destPos + ", mooves = " + string.Join(", ", dto.mooves));
            ihm.board.RefreshDTOPawn();
            ihm.gestionDTO.moveDTOPawn(dto);

            // Le joueur viens de d�placer un pion donc sa prochaine action est de d�placer un mur
            int current_player = ihm.GetCurrentPlayer();
            if (ihm.GetPlayer(current_player).verticalWalls > 0 || ihm.GetPlayer(current_player).horizontalWalls > 0)
            {
                ihm.SetPlayerPlacingWall(current_player, true);
            }
            ihm.SwitchActionPlayer();
            ihm.ToggleError(false);
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Applique un DTO etat du jeu
        /// Privee
        /// </summary>
        /// <param name="dto"></param>
        private void applyDTOGameState(Common.DTOGameState dto)
        {
            // Check if there is a winner
            if (dto.winner > 0)
            {
                ihm.endGame(dto.winner);
                // and finish without applying the new GameState
                return;
            }

            // Met le nombre de murs restants selon le joueur
            if (dto.yellowPlayer.isPlaying)
            {
                Debug.Log("applyDTOGameState, yellowPlayer.verticalWalls = " + dto.yellowPlayer.verticalWalls + ", yellowPlayer.horizontalWalls = " + dto.yellowPlayer.horizontalWalls);
                ihm.SetPlayerWalls(1, (int)dto.yellowPlayer.verticalWalls, (int)dto.yellowPlayer.horizontalWalls);
            }
            else
            {
                Debug.Log("applyDTOGameState, redPlayer.verticalWalls = " + dto.redPlayer.verticalWalls + ", redPlayer.horizontalWalls = " + dto.redPlayer.horizontalWalls);
                ihm.SetPlayerWalls(2, (int)dto.redPlayer.verticalWalls, (int)dto.redPlayer.horizontalWalls);
            }

            // Change current player and update Overlay
            int current_player = ihm.GetCurrentPlayer();
            if (dto.yellowPlayer.isPlaying && current_player == 2)
            {
                ihm.SetCurrentPlayer(1);
                ihm.UpdateRemainingWalls(ihm.GetPlayer(1));
            }
            else if (dto.redPlayer.isPlaying && current_player == 1)
            {
                ihm.SetCurrentPlayer(2);
                ihm.UpdateRemainingWalls(ihm.GetPlayer(2));
            }
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Applique un DTO erreur
        /// Privee
        /// TODO : indiquer visuellement qu'il y a eu une erreur
        /// </summary>
        /// <param name="dto"></param>
        private void applyDTOError(Common.DTOError dto)
        {
            Debug.Log("applyDTOError, errorCode = " + dto.errorCode);
            ihm.ToggleError(true);
        }

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Envoie un DTO a la logique de jeu
        /// Publique
        /// </summary>
        /// <param name="dto"></param>
        public void sendDTOToLogic(object dto)
        {
            gamelogic.sendDTO(dto);
        }
    }
}