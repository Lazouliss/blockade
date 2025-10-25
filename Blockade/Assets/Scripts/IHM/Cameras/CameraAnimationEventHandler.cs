using blockade.Blockade_IHM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace blockade.Blockade_IHM
{
    public class CameraAnimationEventHandler : MonoBehaviour
    {
        [SerializeField] IHM ihm;

        /// <summary>
        /// Par Thomas MONTIGNY
        /// 
        /// Gere l'action apres l'animation de fin de partie
        /// 
        /// Publique
        /// </summary>
        public void AnimationEnd()
        {
            Debug.Log("Animation completed!");
            ihm.ShowEndGameMenu();
        }
    }
}