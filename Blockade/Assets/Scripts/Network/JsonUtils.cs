using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using blockade.Blockade_common;
using System.Collections.Generic;
using System;

namespace blockade.Blockade_Online
{    
    public class JsonUtils
    {
        /*public string dtoToJSON<DTO>(DTO dto)
        {
            return JsonUtility.ToJson(dto, true);
            
        }*/

        public static Dictionary<string, string> ParseJsonToDictionary(string jsonString)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            // Remove the curly braces
            jsonString = jsonString.Trim('{', '}');

            // Split the key-value pairs
            string[] pairs = jsonString.Split(new string[] { "\",\"" }, StringSplitOptions.None);

            foreach (string pair in pairs)
            {
                string[] kv = pair.Split(new string[] { "\":\"" }, StringSplitOptions.None);

                // Remove extra quotes from key and value
                string key = kv[0].Trim('\"');
                string value = kv[1].Trim('\"');

                dictionary[key] = value;
            }

            return dictionary;
        }

        public static string dtoWallToJSON(Common.DTOWall dto)
        {
            string json = "{";
            json += "\"coord1\":" + dto.coord1 + ",";
            json += "\"coord2\":" + dto.coord2 + ",";
            json += "\"direction\":" + dto.direction + ",";
            json += "\"isAdd\":" + dto.isAdd ?? null + "\n";
            json += "}";
            return json;
        }
        public static string dtoPawnToJSON(Common.DTOPawn dto)
        {
            string json = "{\n";
            json += "\"startPos\":" + dto.startPos + ",\n";
            json += "\"destPos\":" + dto.destPos + ",\n";

            json += "}";
            return json;
        }
        public static string dtoGameStateToJSON(Common.DTOGameState dto)
        {
            string json = "{\n";
            json += "\"redPlayer\":" + dto.redPlayer + ",\n";
            json += "\"redPlayerHorizontalWalls\":" + dto.redPlayer.horizontalWalls + ",\n";
            json += "\"redPlayerVerticalWalls\":" + dto.redPlayer.verticalWalls + ",\n";
            json += "\"redPlayerIsPlaying\":" + dto.redPlayer.isPlaying + ",\n";
            json += "\"yellowPlayer\":" + dto.yellowPlayer + ",\n";
            json += "\"yellowPlayerHorizontalWalls\":" + dto.yellowPlayer.horizontalWalls + ",\n";
            json += "\"yellowPlayerVerticalWalls\":" + dto.yellowPlayer.verticalWalls + ",\n";
            json += "\"yellowPlayerIsPlaying\":" + dto.yellowPlayer.isPlaying + ",\n";
            json += "\"winner\":" + dto.winner + "\n";
            json += "}";
            return json;
        }
        public static string dtoErrorToJSON(Common.DTOError dto)
        {
            string json = "{\n";
            json += "\"errorCode\":" + dto.errorCode + "\n";
            json += "}";
            return json;
        }

        public static Common.DTOWall jsonToDTOWall_IHM(string json)
        {
            json = json.Replace("{", "").Replace("}", "").Replace("\n", "");
            string[] formattedJSON = json.Split(',');

            uint firstCoordValue1 = 0;
            uint firstCoordValue2 = 0;
            if (formattedJSON[0].IndexOf('(') != -1 && formattedJSON[1].IndexOf(')') != -1)
            {
                firstCoordValue1 = Convert.ToUInt32(formattedJSON[0].Substring(formattedJSON[0].IndexOf('(') + 1));
                firstCoordValue2 = Convert.ToUInt32(formattedJSON[1].Substring(0, formattedJSON[1].IndexOf(')')));
            }
            (uint, uint) firstCoord = (firstCoordValue1, firstCoordValue2);

            uint secondCoordValue1 = 0;
            uint secondCoordValue2 = 0;
            if (formattedJSON[2].IndexOf('(') != -1 && formattedJSON[3].IndexOf(')') != -1)
            {
                secondCoordValue1 = Convert.ToUInt32(formattedJSON[2].Substring(formattedJSON[2].IndexOf('(') + 1));
                secondCoordValue2 = Convert.ToUInt32(formattedJSON[3].Substring(0, formattedJSON[3].IndexOf(')')));
            }
            (uint, uint) secondCoord = (secondCoordValue1, secondCoordValue2);

            string directionString = formattedJSON[4].IndexOf(':') != -1 ? formattedJSON[4].Substring(formattedJSON[4].IndexOf(':') + 1) : null;
            Common.Direction direction = new Common.Direction();
            foreach (Common.Direction dir in Enum.GetValues(typeof(Common.Direction)))
            {
                if (dir.ToString().Equals(directionString))
                {
                    direction = dir;
                }
            }

            bool isAdd = bool.Parse(formattedJSON[5].Substring(formattedJSON[5].IndexOf(':') + 1));
            DTOHandler Handler = new DTOHandler();
            Common.DTOWall dtoWall = Handler.createWallDTO(firstCoord, secondCoord, direction,  isAdd );

            return dtoWall;
        }

        public static Common.DTOPawn jsonToDTOPawn_IHM(string json)
        {
            json = json.Replace("{", "").Replace("}", "").Replace("\n", "");
            string[] formattedJSON = json.Split(',');

            uint startPosValue1 = 0;
            uint startPosValue2 = 0;
            if (formattedJSON[0].IndexOf('(') != -1 && formattedJSON[1].IndexOf(')') != -1)
            {
            startPosValue1 = Convert.ToUInt32(formattedJSON[0].Substring(formattedJSON[0].IndexOf('(') + 1));
            startPosValue2 = Convert.ToUInt32(formattedJSON[1].Substring(0, formattedJSON[1].IndexOf(')')));
            }
            (uint, uint) startPos = (startPosValue1, startPosValue2);

            uint destPosValue1 = 0;
            uint destPosValue2 = 0;
            if (formattedJSON[2].IndexOf('(') != -1 && formattedJSON[3].IndexOf(')') != -1)
            {
            destPosValue1 = Convert.ToUInt32(formattedJSON[2].Substring(formattedJSON[2].IndexOf('(') + 1));
            destPosValue2 = Convert.ToUInt32(formattedJSON[3].Substring(0, formattedJSON[3].IndexOf(')')));
            }
            (uint, uint) destPos = (destPosValue1, destPosValue2);

            Common.DTOPawn dtoPawn = new Common.DTOPawn()
            {
            startPos = startPos,
            destPos = destPos
            };
            return dtoPawn;
        }
    //A corriger car SetHorizontalWalls et SetVerticalWalls n'existe pas
        public static Common.DTOGameState jsonToDTOGameState(string json)
         {
            json = json.Replace("{", "").Replace("}", "").Replace("\n", "");
            string[] formattedJSON = json.Split(',');

            Player playerY = new Player("yellowPlayer", 1);
            Player playerR = new Player("redPlayer", 2);

            uint redPlayerHorizontalWalls = Convert.ToUInt32(formattedJSON[1].Substring(formattedJSON[1].IndexOf(':') + 1));
            uint redPlayerVerticalWalls = Convert.ToUInt32(formattedJSON[2].Substring(formattedJSON[2].IndexOf(':') + 1));
            bool redPlayerIsPlaying = bool.Parse(formattedJSON[3].Substring(formattedJSON[3].IndexOf(':') + 1));

            uint yellowPlayerHorizontalWalls = Convert.ToUInt32(formattedJSON[5].Substring(formattedJSON[5].IndexOf(':') + 1));
            uint yellowPlayerVerticalWalls = Convert.ToUInt32(formattedJSON[6].Substring(formattedJSON[6].IndexOf(':') + 1));
            bool yellowPlayerIsPlaying = bool.Parse(formattedJSON[7].Substring(formattedJSON[7].IndexOf(':') + 1));

            uint winner = Convert.ToUInt32(formattedJSON[8].Substring(formattedJSON[8].IndexOf(':') + 1));

            playerR.SetHorizontalWalls(redPlayerHorizontalWalls);
            playerR.SetVerticalWalls(redPlayerVerticalWalls);

            playerY.SetHorizontalWalls(yellowPlayerHorizontalWalls);
            playerY.SetVerticalWalls(yellowPlayerVerticalWalls);

            string currentPlayer = "";
            if (redPlayerIsPlaying)
            {
                currentPlayer = "Red";
            } else if (yellowPlayerIsPlaying)
            {
                currentPlayer = "Yellow";
            }

            DTOHandler Handler = new DTOHandler();
            Common.DTOGameState gameStateDTO = Handler.createGameStateDTO(playerY, playerR, winner, currentPlayer);
             return gameStateDTO;
         }
        public static Common.DTOError jsonToDTOError(string json)
        {
            json = json.Replace("{", "").Replace("}", "").Replace("\n", "");
            string[] formattedJSON = json.Split(',');

            int errorCode = Int32.Parse(formattedJSON[0].Substring(formattedJSON[0].IndexOf(':') + 1));

            DTOHandler Handler = new DTOHandler();
            Common.DTOError errorDTO = Handler.createErrorDTO(errorCode);

            return errorDTO;
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}