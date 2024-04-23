
using blockade.AI.Structure;

namespace blockade.AI.Bots
{
    /// <summary>
    /// An interface that every bot must implement.
    /// By Gabin Maury
    /// </summary>
    internal interface IBlockadeBot
    {
        public Turn getBestTurn(Board board);
        public string getBotName();
    }
}
