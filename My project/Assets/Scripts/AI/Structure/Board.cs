
using blockade.Blockade_common;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace blockade.AI.Structure
{
    /// <summary>
    /// A board is the current game state
    /// Stores:
    ///     - Current positions of pawns
    ///     - Current positions of walls
    ///     - Next player to play
    ///     - All walls
    ///     - Player "inventories"
    /// 
    /// By Gabin Maury (except some functions signed by other people)
    /// </summary>
    public class Board //temporarily public, to be changed to internal
    {
        public Square[] squares;
        public (BoardPos, BoardPos) redPawns;
        public (BoardPos, BoardPos) yellowPawns;
        public List<Wall> walls;
        public (int, int) yellowWallsLeft; //(horizontal, vertical)
        public (int, int) redWallsLeft; //(horizontal, vertical)
        public bool isYellowToPlay;
        public int[] yellowDistanceToObj;
        public int[] redDistanceToObj;

        /// <summary>
        /// Copy constructor (deep copy)
        /// by gabin maury
        /// </summary>
        public Board(Board other)
        {
            this.squares = new Square[other.squares.Length];
            for (int i = 0; i < other.squares.Length; i++)
            {
                this.squares[i] = new Square(other.squares[i]);
            }

            this.redPawns = other.redPawns;
            this.yellowPawns = other.yellowPawns;

            this.walls = new List<Wall>();
            foreach (Wall wall in other.walls)
            {
                this.walls.Add(new Wall(wall));
            }

            this.yellowWallsLeft = other.yellowWallsLeft;
            this.redWallsLeft = other.redWallsLeft;
            this.isYellowToPlay = other.isYellowToPlay;
            this.yellowDistanceToObj = (int[])other.yellowDistanceToObj.Clone();
            this.redDistanceToObj = (int[])other.redDistanceToObj.Clone();
        }


        /// <summary>
        /// By Gabin Maury
        /// </summary>
        public Board()
        {
            walls = new List<Wall>();
            squares = new Square[Common.MAP_HEIGHT * Common.MAP_WIDTH];

            for (int i = 0; i < Common.MAP_WIDTH*Common.MAP_HEIGHT; i++) { 
                squares[i] = new Square(i, false);

                if (i < 11) // if bottom of the map
                {
                    squares[i].addWallBelow();
                } //not else if because of corners
                if (i > 154 - 11) //if top of the map
                {
                    squares[i].addWallAbove();
                }
                if (i % 11 == 0) //if left of the map
                {
                    squares[i].addWallLeft();
                }
                if (i % 11 == 10) //if right of the map
                {
                    squares[i].addWallRight();
                }

            }

            redPawns.Item1 = new BoardPos(36);  // red pawn 1
            redPawns.Item2 = new BoardPos(40);  // red pawn 2
            yellowPawns.Item1 = new BoardPos(113); // yellow pawn 1
            yellowPawns.Item2 = new BoardPos(117); // yellow pawn 2

            squares[36].hasPawn = true;
            squares[40].hasPawn = true;
            squares[113].hasPawn = true;
            squares[117].hasPawn = true;

            // precalculated using getMinDistanceToObjective
            yellowDistanceToObj = new int[] { 6, 5, 4, 3, 4, 5, 4, 3, 4, 5, 6, 5, 4, 3, 2, 3, 4, 3, 2, 3, 4, 5, 4, 3, 2, 1, 2, 3, 2, 1, 2, 3, 4, 3, 2, 1, 0, 1, 2, 1, 0, 1, 2, 3, 4, 3, 2, 1, 2, 3, 2, 1, 2, 3, 4, 5, 4, 3, 2, 3, 4, 3, 2, 3, 4, 5, 6, 5, 4, 3, 4, 5, 4, 3, 4, 5, 6, 7, 6, 5, 4, 5, 6, 5, 4, 5, 6, 7, 8, 7, 6, 5, 6, 7, 6, 5, 6, 7, 8, 9, 8, 7, 6, 7, 8, 7, 6, 7, 8, 9, 10, 9, 8, 7, 8, 9, 8, 7, 8, 9, 10, 11, 10, 9, 8, 9, 10, 9, 8, 9, 10, 11, 12, 11, 10, 9, 10, 11, 10, 9, 10, 11, 12, 13, 12, 11, 10, 11, 12, 11, 10, 11, 12, 13 };
            redDistanceToObj = new int[] { 13, 12, 11, 10, 11, 12, 11, 10, 11, 12, 13, 12, 11, 10, 9, 10, 11, 10, 9, 10, 11, 12, 11, 10, 9, 8, 9, 10, 9, 8, 9, 10, 11, 10, 9, 8, 7, 8, 9, 8, 7, 8, 9, 10, 9, 8, 7, 6, 7, 8, 7, 6, 7, 8, 9, 8, 7, 6, 5, 6, 7, 6, 5, 6, 7, 8, 7, 6, 5, 4, 5, 6, 5, 4, 5, 6, 7, 6, 5, 4, 3, 4, 5, 4, 3, 4, 5, 6, 5, 4, 3, 2, 3, 4, 3, 2, 3, 4, 5, 4, 3, 2, 1, 2, 3, 2, 1, 2, 3, 4, 3, 2, 1, 0, 1, 2, 1, 0, 1, 2, 3, 4, 3, 2, 1, 2, 3, 2, 1, 2, 3, 4, 5, 4, 3, 2, 3, 4, 3, 2, 3, 4, 5, 6, 5, 4, 3, 4, 5, 4, 3, 4, 5, 6 };

            yellowWallsLeft = (9, 9);
            redWallsLeft = (9, 9);

        }
        
        // All the folowing functions consider they will recieve valid moves. No checks applied. Wrongly using these functions will lead to unexpected behaivor

        /// <summary>
        /// Plays a turn
        /// 
        /// By Gabin Maury
        /// </summary>
        public void makeTurn(Turn turn)
        {
            makeMove(turn.move);
            placeWall(turn.wall);
             //after we make a turn, it's the other player's turn to play!
        }

        /// <summary>
        /// Plays a pawn move
        /// 
        /// By Gabin Maury
        /// </summary>
        public void makeMove(Move move)
        {
            squares[move.getOld().position].hasPawn = false;
            squares[move.getNew().position].hasPawn = true;

            if (redPawns.Item1.position == move.getOld().position)
            {
                redPawns.Item1 = new BoardPos(move.getNew().position);
            }
            else if(redPawns.Item2.position == move.getOld().position)
            {
                redPawns.Item2 = new BoardPos( move.getNew().position);
            }

            else if(yellowPawns.Item1.position == move.getOld().position)
            {
                yellowPawns.Item1 = new BoardPos(move.getNew().position);
            }
            else if(yellowPawns.Item2.position == move.getOld().position)
            {
                yellowPawns.Item2 = new BoardPos(move.getNew().position);
            }
            else
            {
                //show_current_board();
               Debug.LogErrorFormat("AI ERROR: WTF? old: ({0},{1}), new: ({2},{3})", move.getOld().posX, move.getOld().posY, move.getNew().posX, move.getNew().posY);
            }

        }

        /// <summary>
        /// check if a pawn is on an opposant home square
        /// 0 : not
        /// 1 : yellow victory
        /// 2 : red victory
        /// 
        /// By Guillaume Desir & gabin maury
        /// </summary>

        public int checkVictory(bool quick = true)
        {
            //by gabin maury
            //if no walls are left, there is no strategy possible so we know the shortest path will always win
            //A* implementation blatantly copied from wikipedia, i deserve no credit whatsoever for this
            if(quick && (yellowWallsLeft.Item1 + yellowWallsLeft.Item1 + redWallsLeft.Item1 + redWallsLeft.Item1 == 0))
            {
                int[] pawns = new int[4] { yellowPawns.Item1.position, yellowPawns.Item1.position, redPawns.Item1.position, redPawns.Item2.position };
                int[] distance_to_obj = new int[4] { 100, 100, 100, 100 }; // 100 is way larger than the grid so it can be a start value

                for (int i = 0; i < 4; i++)
                {
                    List<(int node, int distance)> openSet = new List<(int node, int distance)>();
                    HashSet<int> closedSet = new HashSet<int>();

                    openSet.Add((pawns[i], 0));
                    openSet.Sort((a, b) => (a.distance + (i < 2 ? yellowDistanceToObj[a.node] : redDistanceToObj[a.node])).CompareTo(b.distance + (i < 2 ? yellowDistanceToObj[b.node] : redDistanceToObj[b.node])));

                    while (openSet.Count > 0)
                    {
                        (int node, int distance) curr_node = openSet[0];
                        openSet.RemoveAt(0);
                        closedSet.Add(curr_node.node);

                        if (i < 2 && (curr_node.node == 36 || curr_node.node == 40))
                        {
                            distance_to_obj[i] = curr_node.distance;
                            break;
                        }
                        else if (i >= 2 && (curr_node.node == 113 || curr_node.node == 117))
                        {
                            distance_to_obj[i] = curr_node.distance;
                            break;
                        }

                        List<int> neighs = squares[curr_node.node].get_neighbors();
                        foreach (int neigh in neighs)
                        {
                            if (!closedSet.Contains(neigh))
                            {
                                int newDistance = curr_node.distance + 1;
                                openSet.Add((neigh, newDistance));
                                openSet.Sort((a, b) => (a.distance + (i < 2 ? yellowDistanceToObj[a.node] : redDistanceToObj[a.node])).CompareTo(b.distance + (i < 2 ? yellowDistanceToObj[b.node] : redDistanceToObj[b.node])));
                            }
                        }
                    }
                }

                if (Math.Min(distance_to_obj[0], distance_to_obj[1]) < Math.Min(distance_to_obj[2], distance_to_obj[3]))
                {
                    return 1;
                }
                else {  return 2; }


            }

            //by  guillaume desir

            if (redPawns.Item1.position == 113 || redPawns.Item1.position == 117 || squares[redPawns.Item1.position].get_neighbors().Contains(113) || squares[redPawns.Item1.position].get_neighbors().Contains(117))
            {
                return 2;
            }
            else if(redPawns.Item2.position == 113 || redPawns.Item2.position == 117 || squares[redPawns.Item2.position].get_neighbors().Contains(113) || squares[redPawns.Item2.position].get_neighbors().Contains(117))
            {
                return 2;
            }
            else if(yellowPawns.Item1.position == 36 || yellowPawns.Item1.position == 40 || squares[yellowPawns.Item1.position].get_neighbors().Contains(36) || squares[yellowPawns.Item1.position].get_neighbors().Contains(40))
            {
                return 1;
            }
            else if(yellowPawns.Item2.position == 36 || yellowPawns.Item2.position == 40 || squares[yellowPawns.Item2.position].get_neighbors().Contains(36) || squares[yellowPawns.Item2.position].get_neighbors().Contains(40))
            {
                return 1;
            }

            return 0;
        }

        /// <summary>
        /// Undoes a move
        /// 
        /// By Gabin Maury
        /// </summary>
        public void undoMove(Move move)
        {
            makeMove(new Move(move.getNew(), move.getOld())); //just reverse the values
        }

        /// <summary>
        /// Places a wall object on the board (cuts all the right connexions)
        /// 
        /// By Gabin Maury
        /// </summary>
        public void placeWall(Wall wall)
        {
            if(wall.position1 < 0) // if wall is empty, we don't place the wall
            {
                isYellowToPlay = !isYellowToPlay;
                return;
            }
            walls.Add(wall);
            switch (wall.orientation)
            {
                case Wall.Orientation.UP:
                    squares[wall.position1].addWallAbove();
                    squares[wall.position2].addWallAbove();

                    squares[wall.position1 + Common.MAP_WIDTH].addWallBelow();
                    squares[wall.position2 + Common.MAP_WIDTH].addWallBelow();
                    break;
                case Wall.Orientation.RIGHT:
                    squares[wall.position1].addWallRight();
                    squares[wall.position2].addWallRight();

                    squares[wall.position1 + 1].addWallLeft();
                    squares[wall.position2 + 1].addWallLeft();
                    break;
                case Wall.Orientation.DOWN:
                    squares[wall.position1].addWallBelow();
                    squares[wall.position2].addWallBelow();

                    squares[wall.position1 - Common.MAP_WIDTH].addWallAbove();
                    squares[wall.position2 - Common.MAP_WIDTH].addWallAbove();
                    break;
                case Wall.Orientation.LEFT:
                    squares[wall.position1].addWallLeft();
                    squares[wall.position2].addWallLeft();

                    squares[wall.position1 - 1].addWallRight();
                    squares[wall.position2 - 1].addWallRight();
                    break;
            }

            switch(wall.orientation)
            {
                case Wall.Orientation.UP: case Wall.Orientation.DOWN: //horizontal
                    if (isYellowToPlay)
                    {
                        //yellowWalls.Item1[yellowWallsLeft.Item1] = wall;
                        yellowWallsLeft.Item1--;
                    }
                    else
                    {
                        //redWalls.Item1[redWallsLeft.Item1] = wall;
                        redWallsLeft.Item1--;

                    }
                    break;
                case Wall.Orientation.RIGHT: case Wall.Orientation.LEFT: //vertical
                    if (isYellowToPlay){
                        //yellowWalls.Item2[yellowWallsLeft.Item2] = wall;
                        yellowWallsLeft.Item2--;
                    }
                    else
                    {
                        //redWalls.Item2[redWallsLeft.Item2] = wall;
                        redWallsLeft.Item2--;
                    }
                    break;
            }

            isYellowToPlay = !isYellowToPlay;

        }

        /// <summary>
        /// Removes a placed wall
        /// 
        /// By Gabin Maury
        /// </summary>
        public void undoWall(Wall wall)
        {
            int index = walls.FindIndex(w => w == wall);

            if (index != -1)
            {
                walls.RemoveAt(index);
            }
            else
            {
                Debug.LogError("AI ERROR: Tried to undo a wall that did not exist.");
                return;
            }
            switch (wall.orientation)
            {
                case Wall.Orientation.UP:
                    squares[wall.position1].surroundedBy &= ~Square.WALL_ABOVE;
                    squares[wall.position2].surroundedBy &= ~Square.WALL_ABOVE;

                    squares[wall.position1 + Common.MAP_WIDTH].surroundedBy &= ~Square.WALL_BELOW;
                    squares[wall.position2 + Common.MAP_WIDTH].surroundedBy &= ~Square.WALL_BELOW;
                    break;
                case Wall.Orientation.RIGHT:
                    squares[wall.position1].surroundedBy &= ~Square.WALL_RIGHT;
                    squares[wall.position2].surroundedBy &= ~Square.WALL_RIGHT;

                    squares[wall.position1 + 1].surroundedBy &= ~Square.WALL_LEFT;
                    squares[wall.position2 + 1].surroundedBy &= ~Square.WALL_LEFT;
                    break;
                case Wall.Orientation.DOWN:
                    squares[wall.position1].surroundedBy &= ~Square.WALL_BELOW;
                    squares[wall.position2].surroundedBy &= ~Square.WALL_BELOW;

                    squares[wall.position1 - Common.MAP_WIDTH].surroundedBy &= ~Square.WALL_ABOVE;
                    squares[wall.position2 - Common.MAP_WIDTH].surroundedBy &= ~Square.WALL_ABOVE;
                    break;
                case Wall.Orientation.LEFT:
                    squares[wall.position1].surroundedBy &= ~Square.WALL_LEFT;
                    squares[wall.position2].surroundedBy &= ~Square.WALL_LEFT;

                    squares[wall.position1 - 1].surroundedBy &= ~Square.WALL_RIGHT; 
                    squares[wall.position2 - 1].surroundedBy &= ~Square.WALL_RIGHT;
                    break;
            }

            switch (wall.orientation)
            {
                case Wall.Orientation.UP:
                case Wall.Orientation.DOWN: //horizontal
                    if (isYellowToPlay)
                    {
                        yellowWallsLeft.Item1++;
                    }
                    else
                    {
                        redWallsLeft.Item1++;
                    }
                    break;
                case Wall.Orientation.RIGHT:
                case Wall.Orientation.LEFT: //vertical
                    if (isYellowToPlay)
                    {
                        yellowWallsLeft.Item2++;
                    }
                    else
                    {
                        redWallsLeft.Item2++;
                    }
                    break;
            }



        }
        /// <summary>
        /// Returns the distance to the objective square for every square (used for precalculating the greedy first heuristic of the pathfindings.
        /// 
        /// By Gabin Maury
        /// </summary>
        public int[] getMinDistanceToObjective(bool isYellow)
        {
            int[] distances = new int[Common.MAP_WIDTH * Common.MAP_HEIGHT];
            bool[] visited = new bool[Common.MAP_WIDTH * Common.MAP_HEIGHT];
            Queue<int> queue = new Queue<int>();

            for (int i = 0; i < distances.Length; i++)
            {
                distances[i] = -1;
                if (isYellow && ((i == 36) || (i == 40)))
                {
                    distances[i] = 0;
                    queue.Enqueue(i);
                }
                else if (!isYellow && ((i == 113) || (i == 117)))
                {
                    distances[i] = 0;
                    queue.Enqueue(i);
                }
            }

            while (queue.Count > 0)
            {
                int curr_node = queue.Dequeue();
                visited[curr_node] = true;
                List<int> curr_neighbors_pos = squares[curr_node].get_neighbors();
                foreach (int neigh_pos in curr_neighbors_pos)
                {
                    if (!visited[neigh_pos])
                    {
                        visited[neigh_pos] = true;
                        queue.Enqueue(neigh_pos);
                        distances[neigh_pos] = distances[curr_node] + 1;
                    }
                }
            }

            return distances;
        }

        /// <summary>
        /// Returns true if a path between pos and the objective of the color (true = yellow, false = red)
        /// 
        /// Uses greedy best-first search with distance to objective heuristic for ~500x speedup on getBestWalls (lol)
        /// 
        /// Might need to change algorithm to make it even faster.
        /// 
        /// By Gabin Maury
        /// </summary>

        public bool isWinAccessible(int pos, bool isYellow)
        {
            List<int> visited = new List<int>();
            SortedSet<Tuple<int, int>> queue = new SortedSet<Tuple<int, int>>(Comparer<Tuple<int, int>>.Create((a, b) => a.Item2.CompareTo(b.Item2)));

            if (isYellow)
            {
                queue.Add(new Tuple<int, int>(pos, yellowDistanceToObj[pos]));
            }
            else
            {
                queue.Add(new Tuple<int, int>(pos, redDistanceToObj[pos]));
            }

            while (queue.Count > 0)
            {
                var curr_node = queue.Min;
                queue.Remove(curr_node);

                if (isYellow && ((curr_node.Item1 == 36) || (curr_node.Item1 == 40)))
                    return true;
                if (!isYellow && ((curr_node.Item1 == 113) || (curr_node.Item1 == 117)))
                    return true;

                visited.Add(curr_node.Item1);

                List<int> curr_neighbors_pos = squares[curr_node.Item1].get_neighbors();
                foreach (int neigh_pos in curr_neighbors_pos)
                {
                    if (!visited.Contains(neigh_pos))
                    {
                        if (isYellow)
                        {
                            queue.Add(new Tuple<int, int>(neigh_pos, yellowDistanceToObj[neigh_pos]));
                        }
                        else
                        {
                            queue.Add(new Tuple<int, int>(neigh_pos, redDistanceToObj[neigh_pos]));
                        }
                    }
                }
            }
            return false;
        }


        //overload for syntax sugar
        public bool isWinAccessible(BoardPos pos, bool isYellow)
        {
            return isWinAccessible(pos.position, isYellow);
        }

        /// <summary>
        /// Returns a list of all legal walls
        /// 
        /// By Gabin Maury
        /// </summary>
        /// <returns></returns>
        public List<Wall> getLegalWalls()
        {
            List<Wall> candidates = new List<Wall>();

            //precalculate distances for fast pathfind when we check if a wall blocks.
            //commented out for now, it seems slower in most cases
            //yellowDistanceToObj = getMinDistanceToObjective(true);
            //redDistanceToObj = getMinDistanceToObjective(false);



            //populate candidates

            //check if horizontal wall does not cut the side of the map or if vertical wall overlaps with left border
            //(position % 11 != 10)

            //check if vertical wall cuts the side of the map or if horizontal wall overlaps with top of the map
            //(position <= 153-11) 

            for (int i = 0; i < 11*14; ++i )
            {
                bool WALL_WITHIN_BOUNDS = (i % 11 != 10 && i <= 153 - 11);
                if (WALL_WITHIN_BOUNDS)
                {
                    if((isYellowToPlay && yellowWallsLeft.Item1 > 0) || (!isYellowToPlay && redWallsLeft.Item1 > 0))
                    {
                        candidates.Add(new Wall(Wall.Orientation.UP, i));
                    }
                    if ((isYellowToPlay && yellowWallsLeft.Item2 > 0) || (!isYellowToPlay && redWallsLeft.Item2 > 0))
                    {
                        candidates.Add(new Wall(Wall.Orientation.RIGHT, i));
                    } 
                }
            }


            //remove unsuitable candidates

            //check if candidates cut or overlap with an other wall

            //overlap
            List<int> unsuitable_candidates_overlap = new List<int>();
            for (int i = 0; i < candidates.Count; ++i)
            {
                Wall wall = candidates[i];
                switch (wall.orientation) // We only check right and up because we only add those
                {
                    case Wall.Orientation.UP:
                        if (squares[wall.position1].hasWallAbove() || squares[wall.position2].hasWallAbove())
                        {
                            unsuitable_candidates_overlap.Add(i);
                        }
                        break;
                    case Wall.Orientation.RIGHT:
                        if (squares[wall.position1].hasWallRight() || squares[wall.position2].hasWallRight())
                        {
                            unsuitable_candidates_overlap.Add(i);

                            //Console.WriteLine("(" + wall.pos1X + "," + wall.pos1Y + "),(" + wall.pos2X + "," + wall.pos2Y + ") is unsuitable (vertical overlap)\n");
                        }
                        break;
                    
                }
            }

            for (int i = unsuitable_candidates_overlap.Count - 1; i > 0; i--) //iterate in reverse because else we would break the indices by removing elements in the beggining of the list
            {
                candidates.RemoveAt(unsuitable_candidates_overlap[i]);
            }

            //cut
            List<int> unsuitable_candidates_cuts = new List<int>();
            for (int i = 0; i < candidates.Count; ++i)
            {
                Wall wall = candidates[i];
                switch (wall.orientation) // We only check right and up because we only add those
                {
                    case Wall.Orientation.UP:
                        if (squares[wall.position1].hasWallRight() && squares[wall.position1 + Common.MAP_WIDTH].hasWallRight()) // if the pos1 (position at the left of the wall) and the square above it both already have a wall there, it means we cut over a wall.
                        {
                            unsuitable_candidates_cuts.Add(i);
                        }
                        break;
                    case Wall.Orientation.RIGHT:
                        if (squares[wall.position1].hasWallAbove() && squares[wall.position1 + 1].hasWallAbove()) //same principle as with UP
                        {
                            unsuitable_candidates_cuts.Add(i);
                        }
                        break;
                }
            }

            
            

            for (int i = unsuitable_candidates_cuts.Count - 1; i >= 0; i--) //iterate in reverse because else we would break the indices by removing elements in the beggining of the list
            {
                candidates.RemoveAt(unsuitable_candidates_cuts[i]);
            }



            //check if candidates block the path of one of the pawns (we do this last because it's the most expensive, we should do it when we have the least possible amount of candidates)
            //uses BFS for now (quick and easy to implement)

            //optimisations for later:

            //memoisation (possible?)
            //connected walls and connected subgraphs

            //maybe unknown graph algorithms with graph cuts or something
            List<int> unsuitable_candidates_blocks = new List<int>();
            for(int i = 0; i < candidates.Count; ++i)
            {
                Wall wall = candidates[i];

                //Console.WriteLine("Testing wall (" + wall.position1 + "," + wall.position2 + ")");

                //check if wall connects two other walls, if it's not, it means we can directly go to the next wall because it's not "closing" any path
                int connected_wall_count = 0;
                switch (wall.orientation)
                {
                    case Wall.Orientation.UP:
                        //perpendicular walls below
                        if (squares[wall.position1].hasWallLeft()) { connected_wall_count++; }
                        if (squares[wall.position2].hasWallLeft()) { connected_wall_count++; }
                        if (squares[wall.position2].hasWallRight()) { connected_wall_count++; }

                        //perpendicular walls above
                        if (squares[wall.position1 + Common.MAP_WIDTH].hasWallLeft()) { connected_wall_count++; }
                        if (squares[wall.position2 + Common.MAP_WIDTH].hasWallLeft()) { connected_wall_count++; }
                        if (squares[wall.position2 + Common.MAP_WIDTH].hasWallRight()) { connected_wall_count++; }

                        //wall on sides "continuing" the wall
                        if (wall.position2 % Common.MAP_WIDTH != Common.MAP_WIDTH - 1)
                        {
                            if (squares[wall.position2 + 1].hasWallAbove()) { connected_wall_count++; }
                        }

                        if (wall.position1 % Common.MAP_WIDTH != 0)
                        {
                            if (squares[wall.position1 - 1 ].hasWallAbove()) { connected_wall_count++; }
                        }


                        break;
                    case Wall.Orientation.RIGHT:
                        //perpendicular walls on the left
                        if (squares[wall.position1].hasWallAbove()) { connected_wall_count++; }
                        if (squares[wall.position2].hasWallAbove()) { connected_wall_count++; }
                        if (squares[wall.position2].hasWallBelow()) { connected_wall_count++; }

                        //perpendicular walls above
                        if (squares[wall.position1 + 1].hasWallAbove()) { connected_wall_count++; }
                        if (squares[wall.position2 + 1].hasWallAbove()) { connected_wall_count++; }
                        if (squares[wall.position2 + 1].hasWallBelow()) { connected_wall_count++; }

                        //wall on sides "continuing" the wall
                        if (wall.position2 + Common.MAP_HEIGHT < Common.MAP_HEIGHT * Common.MAP_WIDTH)
                        {
                            if (squares[wall.position2 + Common.MAP_WIDTH].hasWallRight()) { connected_wall_count++; }
                        }

                        if (wall.position1 - Common.MAP_HEIGHT > 0)
                        {
                            if (squares[wall.position1 - Common.MAP_WIDTH].hasWallRight()) { connected_wall_count++; }
                        }
                        break;

                }

                
                

                if (connected_wall_count >= 2) // if the wall potentially blocks a path, we check it with isWinAccessible
                {

                    //Console.WriteLine("Potentially blocking. Testing...");


                    Board tempBoard = new Board(this);
                    tempBoard.placeWall(wall);
                    bool is_yellow_pawn_blocked = !(tempBoard.isWinAccessible(yellowPawns.Item1.position, true) && (tempBoard.isWinAccessible(yellowPawns.Item2.position, true)));
                    bool is_red_pawn_blocked = !(tempBoard.isWinAccessible(redPawns.Item1.position, false) && (tempBoard.isWinAccessible(redPawns.Item2.position, false)));

                    if (is_red_pawn_blocked || is_yellow_pawn_blocked)
                    {
                        unsuitable_candidates_blocks.Add(i);    
                    }

                }

            }
            for (int i = unsuitable_candidates_blocks.Count - 1; i > 0; i--) //iterate in reverse because else we would break the indices by removing elements in the beggining of the list
            {
                candidates.RemoveAt(unsuitable_candidates_blocks[i]);
            }




            return candidates;
        }

        /// <summary>
        /// Runs a BFS of max depth 2 for every pawn.
        /// Checks if a pawn is blocked by an other pawn, if so it is allowed to move to a distance of one instead of the usual two
        /// If it has all moves ready, the pawn can jump over all pawns in its way
        /// By Gabin Maury
        /// </summary>
        /// <returns>List tuple of ints represeting the previous position and new position <returns>
        public List<Move> getLegalMoves()
        {
            List<Move> ret = new List<Move>();

            List<BoardPos> pawns = new List<BoardPos>() { yellowPawns.Item1, yellowPawns.Item2, redPawns.Item1, redPawns.Item2};

            (int, int) pawn_range; //0, 1 for yellow -- 2,3 for red 

            if (isYellowToPlay)
            {
                pawn_range = (0, 1);
            }
            else
            {
                pawn_range = (2, 3);
            }

            for (int i = pawn_range.Item1; i <= pawn_range.Item2; i++)
            {
                List<int> visited = new List<int> { pawns[i].position };
                Queue<(int node, int dist, bool jumping)> queue = new Queue<(int node, int dist, bool jumping)>();
                queue.Enqueue((pawns[i].position, 0, false));
                const int max_dist = 2;

                while (queue.Count > 0)
                {
                    (int curr_node, int dist_travelled, bool is_jumping) = queue.Dequeue();


                    List<int> curr_neighbors_pos = squares[curr_node].get_neighbors();
                    foreach (int neigh_pos in curr_neighbors_pos)
                    {
                        bool neigh_win = false;
                        if (isYellowToPlay)
                        {
                            neigh_win = neigh_pos == 36 || neigh_pos == 40;
                        }
                        else
                        {
                            neigh_win = neigh_pos == 113 || neigh_pos == 117;
                        }
                        bool neigh_visited = visited.Contains(neigh_pos);
                        bool neigh_has_pawn = !neigh_visited && squares[neigh_pos].hasPawn; // neigh not visited because we are looking at new squares, not the old ones we already checked
                        bool can_jump = (dist_travelled == 0) && neigh_has_pawn; // we can only jump if we have used no moves yet and there is a pawn to jump over
                        bool neigh_blocks = neigh_has_pawn && curr_neighbors_pos.Count == 2; //if there are only two ways, it means that we are in a corridor and if there is a pawn, it blocks us 
                        if (!neigh_visited && (!neigh_has_pawn || can_jump || is_jumping)) //if has pawn, it's considered as if it was not a neighbor (as if there was a wall), unless we can jump or are currently jumping
                        //if (!neigh_visited && !neigh_has_pawn)
                        {
                            visited.Add(neigh_pos);
                            if (is_jumping && !neigh_has_pawn) //if we are currently jumping but the neighboor has no pawn, the jump is over, we will land on the neighbor
                            {
                                ret.Add(new Move(new BoardPos(pawns[i].position), new BoardPos(neigh_pos)));
                            }
                            else if((can_jump || is_jumping) && neigh_has_pawn) //we are jumping, we can bypass the limit of dist, but if neigh has no pawn, then the jump ends.
                            {
                                queue.Enqueue((neigh_pos, dist_travelled + 1, true));
                            }
                            else if (dist_travelled < max_dist) //if we are at the limit, we don't want to visit neighbors
                            {
                                queue.Enqueue((neigh_pos, dist_travelled + 1, false));
                            }
                            if (dist_travelled + 1 == max_dist) //if neigh node is at max distance (== moved by 2 squares), we can add it to te legal moves list
                            {
                                ret.Add(new Move(new BoardPos(pawns[i].position), new BoardPos(neigh_pos)));
                            }
                        }

                        //replace neigh_block with neigh_has_pawn if it turns out the rule is different
                        if (dist_travelled + 1 <= max_dist && neigh_blocks) // if neighbor has pawn blocking us and it would be our only way out in this direction, we can stop after less than max move
                        {
                            ret.Add(new Move(new BoardPos(pawns[i].position), new BoardPos(curr_node)));
                        }

                        if (dist_travelled + 1 <= max_dist && neigh_win)
                        {
                            ret.Add(new Move(new BoardPos(pawns[i].position), new BoardPos(curr_node)));
                        }




                    }
                }
            }

            return ret;

        }


        /// <summary>
        /// Find the shortestPath to win
        /// 
        /// By Matteo Cellarius
        /// </summary>
        /// <returns> The list of moves for a pawns <returns>
        public List<int> GetShortestPath(int source, int target)
        {
            List<int> visited = new List<int>();
            Dictionary<int, int> cameFrom = new Dictionary<int, int>(); // Path
            Queue<int> queue = new Queue<int>();
            queue.Enqueue(source);
            cameFrom[source] = -1;

            while (queue.Count > 0)
            {
                int curr_node = queue.Dequeue();

                if (curr_node == target)
                    break; // Target reached

                visited.Add(curr_node);

                List<int> curr_neighbors_pos = squares[curr_node].get_neighbors();
                foreach (int neigh_pos in curr_neighbors_pos)
                {
                    if (!visited.Contains(neigh_pos) && !cameFrom.ContainsKey(neigh_pos))
                    {
                        queue.Enqueue(neigh_pos);
                        cameFrom[neigh_pos] = curr_node;
                    }
                    else
                    {
                        //Condition if HasPawn
                    }
                }
            }

            // recreate the path
            List<int> path = new List<int>();
            int currentPathNode = target;
            while (currentPathNode != -1)
            {
                path.Add(currentPathNode);
                currentPathNode = cameFrom.ContainsKey(currentPathNode) ? cameFrom[currentPathNode] : -1;
            }
            path.Reverse(); // Invert the path

            return CleanMove(path);
        }

        /// By Matteo Cellarius
        public List<int> CleanMove(List<int> path)
        {
            if (path.Count <= 2)
                return path; // nothing to clean

            List<int> cleanedPath = new List<int>();
            cleanedPath.Add(path[0]);

            for (int i = 1; i < path.Count - 1; i++) // Exclude last two element cause we can make a 1 case move
            {
                int prev = cleanedPath[cleanedPath.Count - 1];
                int current = path[i];

                if (!AreNeighbors(prev, current)) // check if neighbors
                {
                    cleanedPath.Add(current); // add if not neighbors
                }
            }

            cleanedPath.Add(path[path.Count - 1]);

            return cleanedPath;
        }

        /// By Matteo Cellarius
        private bool AreNeighbors(int a, int b)
        {
            List<int> neighborsA = squares[a].get_neighbors();
            return neighborsA.Contains(b); // check if neighbors
        }

        public List<Turn> getLegalTurns()
        {
            List<Turn> ret = new List<Turn>();
            List<Move> legal_moves = getLegalMoves();
            List<Wall> legal_walls = getLegalWalls();

            if (legal_walls.Count == 0)
            {
                Wall empty_wall = new Wall(Wall.Orientation.UP, -1);
                legal_walls.Add(empty_wall);
            }

            foreach(Move this_move in legal_moves) 
            { 
                foreach( Wall this_wall in legal_walls)
                {
                    ret.Add(new Turn() { move = this_move, wall = this_wall });
                }
            }

            return ret;
        }

    }//class
}//namespace