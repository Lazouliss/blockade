namespace blockade.AI.Structure
{
    /// <summary>
    /// Represents a pawn moving from one square to an other (exists to prevent using tuples everywhere)
    /// By Gabin Maury
    /// </summary>
    public class Move //temporarily public, to be changed to internal
    {
        BoardPos _oldPawn;
        BoardPos _newPawn;

        public Move(BoardPos old_pawn, BoardPos new_pawn)
        {
            _oldPawn = old_pawn;
            _newPawn = new_pawn;
        }

        public BoardPos getOld()
        {
            return _oldPawn;
        }

        public BoardPos getNew(){
            return _newPawn;
        }
    }
}
