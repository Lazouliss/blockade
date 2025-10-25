using blockade.AI.Structure;

namespace blockade.AI.Bots
{
    /// <summary>
    /// Min-Max Algorithm
    /// 
    /// By Chloe
    /// </summary>
    internal class MinMaxBot : IBlockadeBot
    {
        public MinMaxBot() { }

        public Turn getBestTurn(AIBoard board)
        {
            return new Turn();
        }

        public string getBotName()
        {
            return "MinMaxBot";
        }
    }
}
