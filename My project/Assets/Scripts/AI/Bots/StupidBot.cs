using System;
using System.Collections.Generic;
using blockade.AI.Structure;

namespace blockade.AI.Bots
{
    /// <summary>
    /// This bot is very stupid, it only plays randomly
    /// For debug purposes
    /// By Gabin Maury
    /// </summary>
    internal class StupidBot : IBlockadeBot
    {


        public Turn getBestTurn(AIBoard board)
        {
            //return random value
            List<Turn> legal_turns = board.getLegalTurns();

            Random random = new Random();
            Turn random_turn = legal_turns[random.Next(legal_turns.Count)];
            return random_turn;
        }

        public string getBotName()
        {
            return "StupidBot";
        }

        
    }
}
