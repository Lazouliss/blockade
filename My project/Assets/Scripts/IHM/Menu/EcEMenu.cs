using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public void setIALevel1 (int level)
    {
        MenuGlobal.setIALevel(levelValue1, levelString1, level);
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui set le niveau de l'IA 2
    /// Publique
    /// </summary>
    /// <returns></returns>
    public void setIALevel2 (int level)
    {
        MenuGlobal.setIALevel(levelValue2, levelString2, level);
    }
}
