using UnityEngine;
using WebSocketSharp;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using blockade.Blockade_common;
using blockade.Blockade_Online;

public class LobbyClient : MonoBehaviour
{

    public class JsonClientRequest
    {
        public string token { get; set; }
        public string requestType { get; set; }
        public string userId { get; set; }
        public string data { get; set; }
    }
    public class JsonServerResponse
    {
        public int result { get; set; }
        public string responseType { get; set; }
        public string data { get; set; }
    }
    public class JsonServerRequest
    {
        public string requestType { get; set; }
        public string data { get; set; }
    }

    // Exemple de DTOWall
    Common.DTOWall wallDTO = new Common.DTOWall
    {
        coord1 = (3, 4),
        coord2 = (5, 6),
        direction = Common.Direction.UP,
    };

    // Exemple de DTOPawn
    Common.DTOPawn pawnDTO = new Common.DTOPawn
    {
        startPos = (1, 1),
        mooves = new List<Common.Direction> { Common.Direction.UP, Common.Direction.RIGHT, Common.Direction.DOWN }
    };

    private WebSocket ws;
    //private string password = "#UFIPHV";
    private string guestId = "5200";
    private string hostId = "5201";
    private string globalToken = "0x0";


    void Start()
    {
        ws = new WebSocket("ws://localhost:27015");

        ws.OnMessage += (sender, e) => 
        {

            // All messages received from the sockets
            Debug.Log("Received: " + e.Data);
            string text = "{\"requestType\":\"lobby-join\",\"data\":\"GUEST1\"}";
            if(e.Data == text) {
                Debug.Log("alo");
                endAction(hostId, wallDTO, globalToken);
            }
            JObject jsonObject = JObject.Parse(e.Data);
            string[] data = null;
            string responseType = null;
            string requestType = null;

            if(jsonObject.ContainsKey("requestType")) {
                requestType = jsonObject["requestType"].ToString();
            }
            if(jsonObject.ContainsKey("responseType")) {
                responseType = jsonObject["responseType"].ToString();
            }
            if (jsonObject.ContainsKey("data")) {
                Debug.Log("Data: " + jsonObject["data"]);
                data = jsonObject["data"].ToObject<string[]>();
            }
            if(responseType != null)
            {
                switch(responseType) {
                    case "lobby-host":
                        sendInvite(data[0], hostId, globalToken);
                        break;
                    case "lobby-join":
                        SendMessageToLobby("SALUT EH OH", hostId, globalToken);
                        break;
                }
            } else {
                switch(requestType) {
                    case "friend-request":
                        Debug.Log("Friend request received");
                        break;
                    case "lobby-invite":
                        Debug.Log("Lobby invitation received");
                        acceptInvite(data[0], guestId, globalToken);
                        break;
                    case "action-end":
                        Debug.Log("Action ended");
                        // TODO Gérer l'action de l'autre joueur
                        break;
                }
            }
        };

        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("Connected to server");
            //sendRequest(hostId, globalToken);
            //HostLobby(hostId, globalToken);
            
        };

        ws.Connect();
    }

    private void JoinLobby(string userId, string password, string globalToken)
    {
        Debug.Log("Enter lobby password:");
        
        var request = new
        {
            token = globalToken,
            requestType = "lobby-join",
            userId = userId,
            data = password
        };

        ws.Send(JsonConvert.SerializeObject(request));
        Debug.Log("Attempting to join lobby...");
    }

    
    private void HostLobby(string userId, string globalToken)
    {
        var request = new
        {
            token = globalToken,
            requestType = "lobby-host",
            userId = userId,
            data = ""
        };

        ws.Send(JsonConvert.SerializeObject(request));
        Debug.Log("Hosting a new lobby...");
    }

    private void SendMessageToLobby(string message, string userId, string globalToken)
    {
        var messageRequest = new
        {
            token = globalToken,
            requestType = "lobby-send-msg",
            userId = userId,
            data = message
        };

        ws.Send(JsonConvert.SerializeObject(messageRequest));
        Debug.Log("Sending message to lobby: " + message);
    }

    private void sendInvite(string password, string userId, string globalToken){
        var request = new
        {
            token = globalToken,
            requestType = "lobby-invite",
            userId = userId,
            data = new List<string> { "2700", password } // à changer pour remplacer 2700 par l'userId de l'autre joueur
        };
        ws.Send(JsonConvert.SerializeObject(request));
        Debug.Log("Lobby invitation sent");
    }

    private void sendRequest(string userId, string globalToken){
        var request = new 
        {
            token = globalToken,
            requestType = "friend-request",
            userId = userId,
            data = "2700"
        };
        ws.Send(JsonConvert.SerializeObject(request));
        Debug.Log("Friend request sent");
    }

    private void acceptInvite(string password, string userId, string globalToken){
        var request = new 
        {
            token = "0x0",
            requestType = "lobby-join",
            userId = userId,
            data = password
        };
        ws.Send(JsonConvert.SerializeObject(request));
        Debug.Log("Lobby invitation accepted");
    }

    public void StartGame(string userId, string globalToken)
    {
        var request = new
        {
            token = globalToken,
            requestType = "lobby-start-game",
            userId = userId
        };

        ws.Send(JsonConvert.SerializeObject(request));
        Debug.Log("Starting game...");
    }

    public void endAction(string userId, object DTO, string globalToken)
    {
        object DTOToSend;
        switch (DTO.GetType())
        {
            case Type t when t == typeof(Common.DTOWall):
                DTOToSend = JsonUtils.dtoWallToJSON((Common.DTOWall)DTO);
                break;
            case Type t when t == typeof(Common.DTOPawn):
                DTOToSend = JsonUtils.dtoPawnToJSON((Common.DTOPawn)DTO);
                break;
            default:
                DTOToSend = null;
                break;
        }
        var request = new
        {
            token = globalToken,
            requestType = "action-end",
            userId = userId,
            data = DTOToSend
        };

        ws.Send(JsonConvert.SerializeObject(request));
        Debug.Log("Ending action...");
    }

    public void InviteFriend(string inviteUserId,string invitedUserId, string globalToken)
    {
        var request = new
        {
            token = globalToken,
            requestType = "friend-accept",
            userId = inviteUserId,
            data = invitedUserId
        };

        ws.Send(JsonConvert.SerializeObject(request));
        Debug.Log("Sending invite...");
    }

    void OnDestroy()
    {
        if (ws != null)
            ws.Close();
    }
}