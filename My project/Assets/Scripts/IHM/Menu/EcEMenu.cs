using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EcEMenu : MonoBehaviour
{
    public int levelValue1;
    public string levelString1;
    public int levelValue2;
    public string levelString2;

    public void PlayGame ()
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
        public void getIALevel1(int level)
        {
            levelValue1 = UIManager.getIALevel(level).Item1;
            levelString1 = UIManager.getIALevel(level).Item2;
            Debug.Log("levelValue1 : " + levelValue1);
            Debug.Log("levelString1 : " + levelString1);
        }

        /// <summary>
        /// Par Martin GADET
        /// Méthode qui set le niveau de l'IA 2
        /// Publique
        /// </summary>
        /// <returns></returns>
        public void getIALevel2(int level)
        {
            levelValue2 = UIManager.getIALevel(level).Item1;
            levelString2 = UIManager.getIALevel(level).Item2;
            Debug.Log("levelValue2 : " + levelValue2);
            Debug.Log("levelString2 : " + levelString2);
        }
    }

    public void setIALevel1 (int level)
    {
        switch(level)
        {
            case 0:
                levelValue1 = 1;
                levelString1 = "Facile";
                break;
            case 1:
                levelValue1 = 2;
                levelString1 = "Moyen";
                break;
            case 2:
                levelValue1 = 3;
                levelString1 = "Difficile";
                break;
        }

        Debug.Log(levelValue1);
        Debug.Log(levelString1);
    }

    public void setIALevel2 (int level)
    {
        switch(level)
        {
            case 0:
                levelValue2 = 1;
                levelString2 = "Facile";
                break;
            case 1:
                levelValue2 = 2;
                levelString2 = "Moyen";
                break;
            case 2:
                levelValue2 = 3;
                levelString2 = "Difficile";
                break;
        }

        Debug.Log(levelValue2);
        Debug.Log(levelString2);
    }
}
