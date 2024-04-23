using blockade.AI.Structure;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System;
using System.Collections.Generic;


namespace blockade.AI.Bots
{   
    /// <summary>
    /// Naive method
    /// 
    /// By Matteo
    /// </summary>
    internal class NaiveBot : IBlockadeBot
    {
        bool isyellow = false;

        public string getBotName()
        {
            return "Naivebot";
        }

        public NaiveBot() { }

        /// By Matteo Cellarius
        public List<int> GetShortestPathPawn(AIBoard board)
        {
            List<int> Bestpath = new List<int>();

            List<(int, int)> AllPath = new List<(int, int)>();

            if (!isyellow)
            {
                // Win position
                (int, int) Win = (113, 117);

                // Add all possible path
                AllPath.Add((board.redPawns.Item1.position, Win.Item1));
                AllPath.Add((board.redPawns.Item1.position, Win.Item2));
                AllPath.Add((board.redPawns.Item2.position, Win.Item1));
                AllPath.Add((board.redPawns.Item2.position, Win.Item2));

                int shortestLength = int.MaxValue;

                // Check all possible path for win
                foreach ((int, int) pathPair in AllPath)
                {
                    // Get current path for BFS
                    List<int> currentPath = board.GetShortestPath(pathPair.Item1, pathPair.Item2);

                    // Check lenght
                    if (currentPath.Count < shortestLength)
                    {
                        shortestLength = currentPath.Count;
                        Bestpath = currentPath; // Update best Path
                    }
                }
            }
            else if (isyellow)
            {
                // Win position
                (int, int) Win = (36, 40);

                // Add all possible path
                AllPath.Add((board.yellowPawns.Item1.position, Win.Item1));
                AllPath.Add((board.yellowPawns.Item1.position, Win.Item2));
                AllPath.Add((board.yellowPawns.Item2.position, Win.Item1));
                AllPath.Add((board.yellowPawns.Item2.position, Win.Item2));

                int shortestLength = int.MaxValue;

                // Check all possible path for win
                foreach ((int, int) pathPair in AllPath)
                {
                    // Get current path for BFS
                    List<int> currentPath = board.GetShortestPath(pathPair.Item1, pathPair.Item2);

                    // Check lenght
                    if (currentPath.Count < shortestLength)
                    {
                        shortestLength = currentPath.Count;
                        Bestpath = currentPath; // Update best Path
                    }
                }
            }
            

            return Bestpath;
        }

        public (List<int>, List<int>) GetShortestPathWall(AIBoard board)
        {
            List<int> Bestpath = new List<int>();
            List<int> Bestpath1 = new List<int>();

            List<(int, int)> AllPath = new List<(int, int)>();
            List<(int, int)> AllPath1 = new List<(int, int)>();

                if (isyellow)
                {
                    // Win position
                    (int, int) Win = (113, 117);

                    // Add all possible path
                    AllPath.Add((board.redPawns.Item1.position, Win.Item1));
                    AllPath.Add((board.redPawns.Item1.position, Win.Item2));
                    AllPath1.Add((board.redPawns.Item2.position, Win.Item1));
                    AllPath1.Add((board.redPawns.Item2.position, Win.Item2));

                    int shortestLength = int.MaxValue;

                    // Check all possible path for win
                    foreach ((int, int) pathPair in AllPath)
                    {
                        // Get current path for BFS
                        List<int> currentPath = board.GetShortestPath(pathPair.Item1, pathPair.Item2);

                        // Check lenght
                        if (currentPath.Count < shortestLength)
                        {
                            shortestLength = currentPath.Count;
                            Bestpath = currentPath; // Update best Path
                        }
                    }

                    int shortestLength1 = int.MaxValue;

                    foreach ((int, int) pathPair in AllPath1)
                    {
                        // Get current path for BFS
                        List<int> currentPath = board.GetShortestPath(pathPair.Item1, pathPair.Item2);

                        // Check lenght
                        if (currentPath.Count < shortestLength1)
                        {
                            shortestLength1 = currentPath.Count;
                            Bestpath1 = currentPath; // Update best Path
                        }
                    }
                }
                else if (!isyellow)
                {
                    // Win position
                    (int, int) Win = (36, 40);

                    // Add all possible path
                    AllPath.Add((board.yellowPawns.Item1.position, Win.Item1));
                    AllPath.Add((board.yellowPawns.Item1.position, Win.Item2));
                    AllPath1.Add((board.yellowPawns.Item2.position, Win.Item1));
                    AllPath1.Add((board.yellowPawns.Item2.position, Win.Item2));

                    int shortestLength = int.MaxValue;

                    // Check all possible path for win
                    foreach ((int, int) pathPair in AllPath)
                    {
                        // Get current path for BFS
                        List<int> currentPath = board.GetShortestPath(pathPair.Item1, pathPair.Item2);

                        // Check lenght
                        if (currentPath.Count < shortestLength)
                        {
                            shortestLength = currentPath.Count;
                            Bestpath = currentPath; // Update best Path
                        }
                    }

                    foreach ((int, int) pathPair in AllPath1)
                    {
                        // Get current path for BFS
                        List<int> currentPath = board.GetShortestPath(pathPair.Item1, pathPair.Item2);

                        // Check lenght
                        if (currentPath.Count < shortestLength)
                        {
                            shortestLength = currentPath.Count;
                            Bestpath1 = currentPath; // Update best Path
                        }
                    }
                }

            return (Bestpath, Bestpath1);
        }

        /// By Matteo Cellarius
        private int GetOpponentShortestPathLength(AIBoard board)
        {
            List<int> shortestPath = GetShortestPathPawn(board);
            int count = 0;

            //Stupid behaviour
            if (shortestPath.Count == 2) 
            {
                count = 5;
            }
            else
            {
                count = shortestPath.Count;
            }

            return count;
        }

        /// By Matteo Cellarius
        private int GetOpponentShortestPathLengthWall(AIBoard board)
        {
            (List<int> shortestPath,List<int> shortestPath1) = GetShortestPathWall(board);
            int countMin = 0;

            countMin = Math.Min(shortestPath.Count, shortestPath1.Count);

            return countMin;
        }

        /// By Matteo Cellarius
        public Wall FindBestWallPlacement(AIBoard board)
        {
            List<Wall> legalWalls = board.getLegalWalls();
            List<Wall> filteredWalls = new List<Wall>(legalWalls);
            Wall bestWall = null;
            int maxExtension = -1;

            //Clear legalWalls
            foreach (Wall boardWall in board.walls)
            {
                foreach (Wall wall in legalWalls)
                {
                    if (wall == boardWall)
                    {
                        filteredWalls.Remove(wall);
                    }
                }
            }

            foreach (Wall wall in filteredWalls)
            {
                AIBoard tempboard = new AIBoard(board);
                tempboard.placeWall(wall); // Temp Wall
                int opponentShortestPathLength = GetOpponentShortestPathLengthWall(tempboard);

                //tempboard.show_current_board();

                int extension = opponentShortestPathLength - GetOpponentShortestPathLengthWall(board);
                if (extension > maxExtension)
                {
                    maxExtension = extension;
                    bestWall = wall;
                }
            }

            return bestWall;
        }

        /// By Matteo Cellarius
        public Turn getBestTurn(AIBoard board)
        {
            isyellow = board.isYellowToPlay;

            List<int> path = new List<int>();

            path = GetShortestPathPawn(board);

            BoardPos pos1 = new BoardPos(path[0]);
            BoardPos pos2 = new BoardPos(path[1]);

            Move move = new Move(pos1, pos2);

            //Wall wall = new Wall(Wall.Orientation.UP,34); //toChange with Function GetNaiveWall

            Wall wall = FindBestWallPlacement(board);

            return new Turn() { move = move, wall = wall };
        }
    }
}