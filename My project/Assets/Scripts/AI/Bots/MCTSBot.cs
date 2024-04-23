using blockade.AI.Structure;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;

namespace blockade.AI.Bots
{
    /// <summary>
    /// Monte-Carlo tree search algorithm
    /// 
    /// By Gabin Maury
    /// </summary>
    internal class MCTSBot : IBlockadeBot
    {
        public MCTSParam parameters;
        SearchNode? treeRoot;
        bool isPlayingAsYellow;
        Dictionary<int, int> nodeCountAtDepth = new Dictionary<int, int>();

        public System.Random random;
        

        public MCTSBot() 
        {
            parameters = new MCTSParam()
            {
                configName = "MCTSdefault",
                maxIterations = 10000000,
                explorationCoef = Math.Sqrt(2) , // apparently this is the default value for UCT according to research
                maxThinkTime = 1000,
                
            };

            isPlayingAsYellow = true;
            random = new System.Random();
        }

        public MCTSBot(MCTSParam param, bool isYellow)
        {
            parameters = param;
            isPlayingAsYellow = isYellow;
            random = new System.Random();
        }

        public string getBotName()
        {
            return parameters.configName;
        }

        private class SearchNode
        {
            public SearchNode? parent;
            public List<SearchNode> children;
            public Board boardState;
            public Turn turnMade;
            public int visits;
            public double score;
            public int depth;

            public SearchNode(SearchNode? parent, Board board, Turn turnMade)
            {
                this.depth = parent != null ? parent.depth + 1 : 0;
                children = new List<SearchNode>();
                this.parent = parent;
                this.turnMade = turnMade;
                this.boardState = board;
                visits = 0;
                score = 0;
            }

            public void initBoard() // we need this because it's very slow to deep copy, so we only do it when we select this node   (well, 4 microseconds, but when we do this 2000+times for each node in the graph it's very slow)
            {
                if (boardState == null)
                {
                    Board parentBoardState = null;
                    if (parent != null) { parentBoardState = parent.boardState; }
                    if (parentBoardState != null) { boardState = new Board(parentBoardState); }
                    else { boardState = new Board(); }

                    boardState.makeTurn(turnMade);
                }
            }

        }

        

        public Turn getBestTurn(Board board)
        {
            treeRoot = new SearchNode(null, board, new Turn() { });

            int total_iterations = 0;
            Stopwatch stopwatch = new Stopwatch();
            Stopwatch stopwatch_iter = new Stopwatch();
            Stopwatch stopwatch_select = new Stopwatch();
            Stopwatch stopwatch_expand = new Stopwatch();
            Stopwatch stopwatch_simulate = new Stopwatch();
            Stopwatch stopwatch_backpropagate = new Stopwatch();


            stopwatch.Start();
            float total_iter_time = 0f;
            float total_select_time = 0f;
            float total_expand_time = 0f;
            float total_simulate_time = 0f;
            float total_backpropagate_time = 0f;
            SearchNode curr_node;
            expand(treeRoot);
            while (total_iterations < parameters.maxIterations && stopwatch.ElapsedMilliseconds < parameters.maxThinkTime)
            {
                stopwatch_iter.Start();
                
                stopwatch_select.Start();
                curr_node = select(treeRoot);
                stopwatch_select.Stop();
                total_select_time += stopwatch_select.ElapsedMilliseconds;
                stopwatch_select.Reset();

                stopwatch_expand.Start();
                expand(curr_node);
                stopwatch_expand.Stop();
                total_expand_time += stopwatch_expand.ElapsedMilliseconds;
                stopwatch_expand.Reset();

                stopwatch_simulate.Start();
                double sim_result = simulate(curr_node);
                stopwatch_simulate.Stop();
                total_simulate_time += stopwatch_simulate.ElapsedMilliseconds;
                stopwatch_simulate.Reset();

                stopwatch_backpropagate.Start();
                backpropagate(curr_node, sim_result);
                stopwatch_backpropagate.Stop();
                total_backpropagate_time += stopwatch_backpropagate.ElapsedMilliseconds;
                stopwatch_backpropagate.Reset();


                total_iterations++;
                stopwatch_iter.Stop();
                total_iter_time += stopwatch_iter.ElapsedMilliseconds;
                stopwatch_iter.Reset();
            }
            stopwatch.Stop();
            stopwatch.Reset();
            Console.WriteLine("In {0}ms, we had time for {1} iterations.", parameters.maxThinkTime, total_iterations);
            Console.WriteLine("Average time in {0} iterations: {1} ms", total_iterations, total_iter_time/total_iterations);

            Console.WriteLine("Average selection  time in {0} iterations: {1} ms", total_iterations, total_select_time / total_iterations);
            Console.WriteLine("Average expansion  time in {0} iterations: {1} ms", total_iterations, total_expand_time / total_iterations);
            Console.WriteLine("Average simulation time in {0} iterations: {1} ms", total_iterations, total_simulate_time / total_iterations);
            Console.WriteLine("Average backprop   time in {0} iterations: {1} ms", total_iterations, total_backpropagate_time / total_iterations);

            Turn best_turn = new Turn();
            int best_score = -1;
            Console.WriteLine(treeRoot.children.Count);
            foreach (SearchNode child in treeRoot.children)
            {
                if (child.visits > best_score)
                {
                    //Console.WriteLine($"new best node Node: {child.GetHashCode()}, Visits: {child.visits}, Wins: {child.wins}");
                    best_score = child.visits;
                    best_turn = child.turnMade;
                }
            }

            for (int i = 0; i <= 50; i++)
            {
                if (nodeCountAtDepth.ContainsKey(i))
                {
                    Console.WriteLine($"Number of nodes at depth {i}: {nodeCountAtDepth[i]}");
                }
                else
                {
                    Console.WriteLine($"Number of nodes at depth {i}: 0");
                }
            }
            return best_turn;
        }

        private void expand(SearchNode node)
        {
            
            node.initBoard();
            Stopwatch stopwatch = new Stopwatch();
            List<string> times = new List<string>();

            if (node.children.Count == 0)
            {
                stopwatch.Start();
                List<Turn> legal_turns = node.boardState.getLegalTurns();
                stopwatch.Stop();
                times.Add($"getLegalTurns: {stopwatch.ElapsedTicks}ticks");
                stopwatch.Reset();

                int n = legal_turns.Count;

                stopwatch.Start();
                while (n > 1)
                {
                    int k = random.Next(n--);
                    Turn temp = legal_turns[n];
                    legal_turns[n] = legal_turns[k];
                    legal_turns[k] = temp;
                }
                stopwatch.Stop();
                times.Add($"Shuffle: {stopwatch.ElapsedTicks}ticks");
                stopwatch.Reset();

                foreach (Turn turn in legal_turns)
                {
                    stopwatch.Start();
                    //Board newboard = new Board(node.boardState);
                    //newboard.makeTurn(turn);
                    node.children.Add(new SearchNode(node, null, turn));
                    stopwatch.Stop();
                    times.Add($"Add SearchNode: {stopwatch.ElapsedTicks}ticks");
                    stopwatch.Reset();
                }
            }

            //Console.WriteLine(string.Join(Environment.NewLine, times));
        }

        private SearchNode select(SearchNode node)
        {
            if(node.children.Count == 0)
            {
                if (nodeCountAtDepth.ContainsKey(node.depth))
                {
                    nodeCountAtDepth[node.depth]++;
                }
                else
                {
                    nodeCountAtDepth.Add(node.depth, 1);
                }
                return node;
            }
            double best_score = Double.MinValue;
            SearchNode best_node = node.children[0];

            foreach (SearchNode curr_child in node.children)
            {
                double curr_score = (curr_child.score / (curr_child.visits + 0.000001)) + (parameters.explorationCoef*(Math.Sqrt(   Math.Log(node.visits) / (curr_child.visits + 0.000001)))); // we add 0.000001, an "epsilon" value to avoid dividing by 0 
                if (curr_score  > best_score)
                {
                    best_node = curr_child;
                    best_score = curr_score;
                }
            }
            //return best_node;
            return select(best_node);
        }

        private double simulate(SearchNode node)
        {
            Board simulationBoard = new Board(node.boardState);
            float time_taken_getting_walls = 0f;
            float time_taken_getting_moves = 0f;
            int turn_count = 0;
            while (simulationBoard.checkVictory() == 0)
            {
                Turn toplay;
                Stopwatch sw1 = Stopwatch.StartNew();
                List<Move> legal_moves = simulationBoard.getLegalMoves();
                sw1.Stop();
                time_taken_getting_moves += sw1.ElapsedTicks;
                sw1.Reset();

                Stopwatch sw2 = Stopwatch.StartNew();
                List<Wall> legal_walls = simulationBoard.getLegalWalls();
                sw2.Stop();
                time_taken_getting_walls += sw2.ElapsedTicks;
                sw2.Reset();
                if(legal_moves.Count == 0) // if no legal moves we were blocked, we then lost lol, big malus for the algo, this is a temporary fix for the legal walls slowness,sometimes may produce illegal moves
                {
                    //if (simulationBoard.isYellowToPlay == isPlayingAsYellow) //if yellow won return positivenb, if red won return negativenb
                    //{
                    return -10;
                    //}
                    //else
                    //{
                    //    return 1;
                    //}
                }
                Move random_move = legal_moves[random.Next(legal_moves.Count)];
                if (legal_walls.Count == 0)
                {
                    Wall empty_wall = new Wall(Wall.Orientation.UP, -1);
                    toplay = new Turn() { move = random_move, wall = empty_wall };
                }
                else
                {
                    Wall random_wall = legal_walls[random.Next(legal_walls.Count)];
                    toplay = new Turn() { move = random_move, wall = random_wall };
                }
                //simulationBoard.show_current_board();
                simulationBoard.makeTurn(toplay);

                
            }
            
            /*Console.WriteLine("Turn to finish simulation: {0}", turn_count);
            Console.WriteLine("Total time getting legal walls: {0} ticks", time_taken_getting_walls);
            Console.WriteLine("Total time getting legal moves: {0} ticks", time_taken_getting_moves);*/

           if((simulationBoard.checkVictory() == 1 && isPlayingAsYellow) || (simulationBoard.checkVictory() == 2 && !isPlayingAsYellow )) //if yellow won return positivenb, if red won return negativenb
           {
                //normalized score between 0.5 and 1 based on how much turns it took us to win
                //36 because each player has 18 walls, so it's who's going to be the first to win.
                //18 because we still won, so we don't want the score to be 0
                return (36 - turn_count) / 18; 
               
            }
            else
            {
                return -(36 - turn_count) / 18;
            }
        }

        private void backpropagate(SearchNode node, double result)
        {
            while (node != null)
            {
                node.visits++;
                node.score += result;
                //Console.WriteLine($"backprop on Node: {node.GetHashCode()}, Visits: {node.visits}, Wins: {node.wins}");
                node = node.parent;
            }
        }


    }
}
 