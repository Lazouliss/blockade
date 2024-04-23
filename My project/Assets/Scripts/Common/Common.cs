namespace blockade.Blockade_common
{
    public class Common
    {

        //Common consts
        public const int MAP_WIDTH = 11;
        public const int MAP_HEIGHT = 14;
        public const int MAX_WALLS = 9;
        public enum Direction
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        }

        public struct DTOWall
        {
            public (int, int) coord1;
            public (int, int) coord2;
            public Direction direction;
            public bool isAdd;
        }

        public struct DTOPawn
        {
            public (int, int) startPos;
            public (int, int) destPos;
        }

        public struct DTOGameState
        {
            public struct Player
            {
                public uint verticalWalls, horizontalWalls;
                public bool isPlaying;
            }
            public Player yellowPlayer, redPlayer;
            public uint winner; // 0 = no winner yet, 1 = yellow player, 2 = red player
        }

        /// <summary>
        /// Error codes:
        /// 0 = Sucess (no error)
        /// 1 = ?
        /// ...
        /// </summary>
        public struct DTOError
        {
            public int errorCode;
        }



    }
}
