using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Chat : MonoBehaviour
{
    public TMP_Text ChatContent;
    public string inputMessage;

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui recupère l'input 
    /// Publique
    /// </summary>
    /// <returns></returns>
    public void setMessage(string input)
    {
        inputMessage = input;
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui crée le message a afficher
    /// Publique
    /// </summary>
    /// <returns>message</returns>
    public string createMessage(string PlayerName, string message)
    {
        string messageToSend = PlayerName + ">" + message + "\n";
        return messageToSend;
    }

    /// <summary>
    /// Par Martin GADET
    /// Méthode qui affiche le message
    /// Publique
    /// </summary>
    /// <returns></returns>
    public void addMessage()
    {
        ChatContent.text += createMessage(ConnectionMenu.PlayerName, inputMessage);
    }
}
