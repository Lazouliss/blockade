using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using blockade.Blockade_common;
using System.Collections.Generic;
using System;
using UnityEngine.Networking;
using blockade.Blockade_Online;

namespace blockade.Blockade_Online
{
    public class Online : MonoBehaviour
    {
        
        private string jwt;

        void Awake()
        {
            DTOHandler dtoHandler = new DTOHandler();
            Common.DTOWall wall = dtoHandler.createWallDTO((1,2), (3,4), Common.Direction.UP, false);
            string dtoWallStr = JsonUtils.dtoWallToJSON(wall);
            Debug.Log(wall.isAdd);
            Debug.Log(JsonUtils.jsonToDTOWall_IHM(dtoWallStr).isAdd);

            Common.DTOPawn pawn = dtoHandler.createPawnDTO((1,2), new List<Common.Direction> { Common.Direction.UP, Common.Direction.RIGHT, Common.Direction.DOWN });
            string dtoPawnStr = JsonUtils.dtoPawnToJSON(pawn);
            Debug.Log(pawn.mooves[0]);
            Debug.Log(JsonUtils.jsonToDTOPawn_IHM(dtoPawnStr).mooves[0]);

        }
        public void Login(string username, string password, Action<bool, string> onComplete)
        {
            StartCoroutine(SendLoginRequest(username, password, onComplete));
        }
        public IEnumerator SendLoginRequest(string username, string password, Action<bool, string> onComplete)
        {
            UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:8000/users/login?name=" + username + "&password=" + password);

            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogError("Login request failed: " + request.error);
            }
            else
            {
                string responseText = request.downloadHandler.text;
                Dictionary<string, string> dictionary = JsonUtils.ParseJsonToDictionary(responseText);

                if (dictionary.TryGetValue("jwt", out jwt))
                {
                    onComplete?.Invoke(true, jwt);
                }
                else
                {
                    onComplete?.Invoke(false, null);
                }
            }

        }

        public void Register(string username, string password, Action<bool> onComplete)
        {
            StartCoroutine(SendRegisterRequest(username, password, onComplete));
        }
        public IEnumerator SendRegisterRequest(string username, string password, Action<bool> onComplete)
        {
            UnityWebRequest request = UnityWebRequest.Post("http://127.0.0.1:8000/users/create?name=" + username + "&password=" + password, "");

            yield return request.SendWebRequest();

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogError("Register request failed: " + request.error);
                onComplete?.Invoke(false);
            }
            else
            {
                onComplete?.Invoke(true);
            }
        }
    }
}