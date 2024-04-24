using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace blockade.Blockade_IHM
{
    public class GameHistoricMenu : MonoBehaviour
    {

        public TMP_Text PseudoTitle;

        /// <summary>
        /// Par Martin GADET
        /// Méthode Start qui appelle displayPlayerName pour afficher le nom du joueur connecté
        /// Publique
        /// </summary>
        /// <returns></returns>
        void Start()
        {
            displayPlayerName(ConnectionMenu.PlayerName);
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode displayPlayerName qui set un nom de joueur au text de PseudoTitle
        /// Publique
        /// </summary>
        /// <returns></returns>
        public void displayPlayerName(string name)
        {
            PseudoTitle.text = name;
        }
    }
}