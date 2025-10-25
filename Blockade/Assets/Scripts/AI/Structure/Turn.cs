

namespace blockade.AI.Structure
{

    /// <summary>
    /// A turn is the combination of a move + placing a wall (to avoid using tuples)
    /// By Gabin Maury
    /// </summary>
    public struct Turn //temporarily public, to be changed to internal
    {
        public Wall wall;
        public Move move;

        public Turn(Turn other)
        {
            this.wall = other.wall;
            this.move = other.move; 
        }
    }
}
