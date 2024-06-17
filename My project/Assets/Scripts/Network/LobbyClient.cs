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
    public string last_message = "";
    public string code = "XXXX";
    public int nbJoueurs = 1;
    public bool hostStarted = false;
    public int gameId = 0;
    private int state = 0;
    private int state_watcher = 0;
    public Action<object, bool> gmDtoSend;
    private object dtoToSend;

    void Start()
    {
        ws = new WebSocket("ws://localhost:27015");

        ws.OnMessage += (sender, e) => 
        {

            // All messages received from the sockets
            // Debug.Log("Received: " + e.Data);
            // string text = "{\"requestType\":\"lobby-join\",\"data\":\"GUEST1\"}";
            // if(e.Data == text) {
            //     Debug.Log("alo");
            //     endAction(hostId, wallDTO, globalToken);
            // }

            JObject jsonObject = JObject.Parse(e.Data);
            string[] data = null;
            string responseType = null;
            string requestType = null;

            Debug.Log("Received: " + jsonObject);

            if(jsonObject.ContainsKey("requestType")) {
                requestType = jsonObject["requestType"].ToString();
            }
            if(jsonObject.ContainsKey("responseType")) {
                responseType = jsonObject["responseType"].ToString();
            }
            // if (jsonObject.ContainsKey("data")) {
            //     Debug.Log("Data: " + jsonObject["data"]);
            //     data = jsonObject["data"].ToObject<string[]>();
            // }
            
            
            Debug.Log("Response type: " + responseType);
            if(responseType != null)
            {
                switch(responseType) {
                    case "lobby-host":
                        code = jsonObject["data"].ToString();
                        break;
                }
            } else {
                switch(requestType) {
                    case "friend-request":
                        Debug.Log("Friend request received");
                        break;
                    case "lobby-invite":
                        Debug.Log("Lobby invitation received");
                        //acceptInvite(data[0], guestId, globalToken);
                        break;
                    case "action-movement":
                        // TODO Gérer l'action de l'autre joueur
                        Debug.Log("Action received");
                        if (state == 0) {
                            dtoToSend = JsonUtils.jsonToDTOPawn_IHM(jsonObject["data"].ToString());
                            state = 1;
                        } else if (state == 1){
                            dtoToSend =  JsonUtils.jsonToDTOWall_IHM(jsonObject["data"].ToString());
                            state = 0;
                            Debug.Log("Action ended");
                        }
                        break;
                    case "lobby-join":
                        nbJoueurs += 1;
                        break;
                    case "lobby-start-game":
                        Debug.Log("Game started");
                        hostStarted = true;
                        gameId = jsonObject["gameId"].ToObject<int>();
                        break;
                    case "lobby-send-msg":
                        Debug.Log("Message received: " + jsonObject["data"]);
                        last_message = jsonObject["data"].ToString();
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

    public void Update(){
        if (state != state_watcher) {
            state_watcher = state;
            Debug.Log("State: " + state);
            Debug.Log("DTO: " + dtoToSend);
            gmDtoSend?.Invoke(dtoToSend, false);
        }
    }

    public void JoinLobby(string userId, string token, string globalToken)
    {
        Debug.Log("Enter lobby password:");
        
        var request = new
        {
            token = globalToken,
            requestType = "lobby-join",
            userId = userId,
            data = token
        };

        ws.Send(JsonConvert.SerializeObject(request));
        Debug.Log("Attempting to join lobby...");
    }

    
    public void HostLobby(string userId, string globalToken)
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

    public void SendMessageToLobby(string message, string userId, string globalToken)
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

    public void sendInvite(string password, string userId, string globalToken){
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

    public void sendRequest(string userId, string globalToken){
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

    public void acceptInvite(string password, string userId, string globalToken){
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

    public void sendAction(string userId, object DTO, string globalToken)
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
            case Type t when t == typeof(Common.DTOGameState):
                DTOToSend = JsonUtils.dtoGameStateToJSON((Common.DTOGameState)DTO);
                break;
            default:
                DTOToSend = null;
                break;
        }
        var request = new
        {
            token = globalToken,
            requestType = "action-movement",
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