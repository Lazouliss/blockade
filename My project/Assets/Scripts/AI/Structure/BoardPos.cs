using blockade.Blockade_common;

namespace blockade.AI.Structure
{
    /// <summary>
    /// The class repesents the position of a pawn.
    /// It is used to easily go from X/Y coordinates to grid position (conversion is automatic) 
    /// By Gabin Maury
    /// </summary>
    public class BoardPos //temporarily public, to be changed to internal
    {
        readonly public int position;
        readonly public int posX;
        readonly public int posY;

        public BoardPos(int position)
        {
            this.position = position;
            posX = position % Common.MAP_WIDTH;
            posY = (int)(position / Common.MAP_WIDTH);
        }

        public BoardPos(int posX, int posY)
        {
            position = posY * Common.MAP_WIDTH + posX;
            this.posX = posX;
            this.posY = posY;
        }

        public static bool operator ==(BoardPos a, BoardPos b)
        {
            //boilerplate to check existance of the objects
            if (object.ReferenceEquals(a, b)) { return true; }
            if (((object)a == null) || ((object)b == null)) { return false; }

            //we only test for position because anyways the coordinates will be infered from the rest
            return a.position == b.position;
        }

        public static bool operator !=(BoardPos a, BoardPos b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            BoardPos other = (BoardPos)obj;
            return position == other.position;
        }

        public override int GetHashCode()
        {
            return position.GetHashCode();
        }
    }
}
