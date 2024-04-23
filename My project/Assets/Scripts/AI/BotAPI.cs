using blockade.AI.Bots;
using blockade.AI.Structure;
using static blockade.Blockade_common.Common;

namespace blockade.AI
{
    /// <summary>
    /// This class deals with all the sending and reception of DTOs
    /// 
    /// By Gabin Maury
    /// </summary>
    public class BotAPI
    {
        ///////////////////////////
        //////////private////////// 
        ///////////////////////////
        //The private methods are utilitary methods to convert data structures to and from DTO more easily

        public Board _board; //to be changed back to private TODO

        private DTOPawn get_dto(Move move)
        {
            return new DTOPawn()
            {
                startPos = ((int)move.getOld().posX, (int)move.getOld().posY),
                destPos = ((int)move.getNew().posX, (int)move.getNew().posY)
            };
        }
            
        private DTOWall get_dto(Wall wall)
        {
            DTOWall.Direction direction;

            switch (wall.orientation)
            {
                case Wall.Orientation.UP:
                    direction = DTOWall.Direction.UP;
                    break;
                case Wall.Orientation.DOWN:
                    direction = DTOWall.Direction.DOWN;
                    break;
                case Wall.Orientation.LEFT:
                    direction = DTOWall.Direction.LEFT;
                    break;
                case Wall.Orientation.RIGHT:
                    direction = DTOWall.Direction.RIGHT;
                    break;
                default:
                    throw new ArgumentException("Invalid wall orientation");
            }

            return new DTOWall()
            {
                coord1 = ((int)wall.pos1X, (int)wall.pos1Y),
                coord2 = ((int)wall.pos2X, (int)wall.pos2Y),
                direction = direction,
                isAdd = true, // always isadd because we use this function to send DTOs
            };
        }

        private Move get_move(DTOPawn dtoPawn)
        {
            BoardPos oldPawn = new BoardPos((int)dtoPawn.startPos.Item1, (int)dtoPawn.startPos.Item2);
            BoardPos newPawn = new BoardPos((int)dtoPawn.destPos.Item1, (int)dtoPawn.destPos.Item2);

            return new Move(oldPawn, newPawn);
        }

        private Wall get_wall(DTOWall dtoWall)
        {
            Wall.Orientation orientation;

            switch (dtoWall.direction)
            {
                case DTOWall.Direction.UP:
                    orientation = Wall.Orientation.UP;
                    break;
                case DTOWall.Direction.DOWN:
                    orientation = Wall.Orientation.DOWN;
                    break;
                case DTOWall.Direction.LEFT:
                    orientation = Wall.Orientation.LEFT;
                    break;
                case DTOWall.Direction.RIGHT:
                    orientation = Wall.Orientation.RIGHT;
                    break;
                default:
                    throw new ArgumentException("Invalid wall direction");
            }
            BoardPos bpos1 = new BoardPos((int)dtoWall.coord1.Item1, (int)dtoWall.coord1.Item2);
            BoardPos bpos2 = new BoardPos((int)dtoWall.coord2.Item1, (int)dtoWall.coord2.Item2);

            Wall wall = new Wall(orientation, bpos1, bpos2);

            return wall;
        }



        ///////////////////////////
        ///////////public////////// 
        ///////////////////////////
        //The public methods are the API that the game logic part will use

        public enum Difficulty{
            EASY,
            MEDIUM,
            HARD,
            DEBUG
        }
        public BotAPI() 
        {
            _board = new Board();
        }

        public BotAPI(Board board) //temporary for testing, will not be in final version TODO
        {
            _board = board;
        }

        public void sendDTO(DTOWall dto_wall)
        {
            Wall wall = get_wall(dto_wall);

            if (dto_wall.isAdd)
            {
                _board.placeWall(wall);
            }
            else
            {
                _board.undoWall(wall);
            }

        }

        public void sendDTO(DTOPawn dto_pawn)
        {
            Move move = get_move(dto_pawn);
            _board.makeMove(move);
        }

        public void sendDTO(DTOGameState dtoGameState)
        {
            // Update yellowWallsLeft and redWallsLeft
            _board.yellowWallsLeft = ((int)dtoGameState.yellowPlayer.verticalWalls, (int)dtoGameState.yellowPlayer.horizontalWalls);
            _board.redWallsLeft = ((int)dtoGameState.redPlayer.verticalWalls, (int)dtoGameState.redPlayer.horizontalWalls);

            // Set isYellowToPlay
            _board.isYellowToPlay = dtoGameState.yellowPlayer.isPlaying;
        }




        public (DTOPawn, DTOWall) get_move(Difficulty difficulty)
        {
            IBlockadeBot bot;
            switch (difficulty)
            {
                case Difficulty.EASY:
                    bot = new NaiveBot();
                    break;

                case Difficulty.MEDIUM:
                    bot = new MinMaxBot();
                    break;

                case Difficulty.HARD:
                    MCTSParam parameters = new MCTSParam()
                    {
                        configName = "default",
                        maxIterations = 1000000,
                        explorationCoef = 0.005,//Math.Sqrt(2), // apparently this is the default value for UCT according to research
                        maxThinkTime = 5000,

                    };
                    bot = new MCTSBot(parameters, _board.isYellowToPlay);
                    break;
                default:
                    bot = new StupidBot();
                    break;
            }

            Turn best_turn = bot.getBestTurn(_board);

            return (get_dto(best_turn.move), get_dto(best_turn.wall));
        }

        //For the framework, maybe to be changed later
        // by Guillaume(s)
        public Turn get_move2(Difficulty difficulty)
        {
            IBlockadeBot bot;
            switch (difficulty)
            {
                case Difficulty.EASY:
                    bot = new NaiveBot();
                    break;

                case Difficulty.MEDIUM:
                    bot = new MinMaxBot();
                    break;

                case Difficulty.HARD:
                    bot = new MCTSBot();
                    break;
                default:
                    bot = new StupidBot();
                    break;
            }

            Turn best_turn = bot.getBestTurn(_board);

            return best_turn;
        }
    }
}
