using UnityEngine;

namespace blockade.Blockade_IHM
{
    public class EcEMenu : MonoBehaviour
    {
        public static int levelValue1;
        public static string levelString1;
        public static int levelValue2;
        public static string levelString2;

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui set le niveau de l'IA 1
        /// Publique
        /// </summary>
        /// <returns></returns>
        public void setIALevel1(int level)
        {
            levelValue1 = UIManager.setIALevel(level).Item1;
            levelString1 = UIManager.setIALevel(level).Item2;
            Debug.Log("levelValue1 : " + levelValue1);
            Debug.Log("levelString1 : " + levelString1);
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui set le niveau de l'IA 2
        /// Publique
        /// </summary>
        /// <returns></returns>
        public void setIALevel2(int level)
        {
            levelValue2 = UIManager.setIALevel(level).Item1;
            levelString2 = UIManager.setIALevel(level).Item2;
            Debug.Log("levelValue2 : " + levelValue2);
            Debug.Log("levelString2 : " + levelString2);
        }
    }
}