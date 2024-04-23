using System.Xml.Serialization;
using blockade.AI.Bots;
using blockade.AI.Structure;
using System.Diagnostics;
using static blockade.Blockade_common.Common;
using System.IO;
using System.Text;


namespace blockade.AI.Tests
{
    /// <summary>
    /// Backend for the advanced mode of AI vs AI.
    /// 
    /// By Guillaume D. & Guillaume C.
    /// </summary>
    public class TestFramework
    {
        private AIBoard _board;
        private IBlockadeBot yellow;
        private IBlockadeBot red;
        private FileStream file;
        //Framework contructor
        // by Guillaume(s)
        public TestFramework(AIBoard board)
        {
            _board = board;
            this.yellow = null;
            this.red = null;
            
        }

        //get player difficulty
        // by Guillaume(s)
        // to redo if usefull

        //Main framework function
        // by Guillaume(s)
        // parameters :
        //      think_time : time alocated to bot
        //      nb_tries : number of time lauch is repeated
        //      initial : true if first lauch
        //      bot1 : bot object for yellow bot
        //      bot2 : bot object for red bot

        internal void LaunchFramework(int think_time = 1000, int nb_tries = 1, bool initial = true, IBlockadeBot bot1 = null, IBlockadeBot bot2 = null)
        {
            this.file = File.Open("test_result.csv", FileMode.Append);
            Stopwatch wholewatch = new Stopwatch();
            Stopwatch turnwatch = new Stopwatch();

            int i = 0;
            string turn = "yellow";
            if (initial)
            {

                if (bot1 == null)
                {
                    bot1 = new StupidBot();
                }
                yellow = bot1;
                if (bot2 == null)
                {
                    bot2 = new StupidBot();
                }
                red = bot2;
                byte[] infoInit = new UTF8Encoding(true).GetBytes("player turn,winner,loser,turn number,duration" + "\n");
                this.file.Write(infoInit, 0, infoInit.Length);
                //this.file.Close();
                //this.file = File.Open("test_result.csv", FileMode.Append);
            }

            wholewatch.Start();
            while (_board.checkVictory() == 0)
            {
                turnwatch.Start();
                i++;
                if (turn == "yellow")
                {
                    turn = "red";
                    _board.makeTurn(yellow.getBestTurn(_board));
                }
                else
                {
                    turn = "yellow";
                    _board.makeTurn(red.getBestTurn(_board));
                }
                turnwatch.Stop();
                turnwatch.Reset();
            }
            wholewatch.Stop();
            //Console.WriteLine("winner : " + turn + " (" + ((turn == "red") ? red.GetType() : yellow.GetType()) + ") en " + i.ToString() + " tours et " + wholewatch.Elapsed.TotalMilliseconds.ToString() + " secondes");
            byte[] info = new UTF8Encoding(true).GetBytes(turn + "," + ((_board.checkVictory() == 1) ? red.getBotName() + "," + yellow.getBotName() : yellow.getBotName() + "," + red.getBotName()) + "," + i.ToString() + "," + wholewatch.Elapsed.TotalMilliseconds.ToString() + "\n");
            this.file.Write(info, 0, info.Length);
            this.file.Close();
            if (nb_tries > 1)
                RestartBoard(new AIBoard(), nb_tries - 1);
        }

        // fonction to restart from a board
        // by Guillaume(s)
        private void RestartBoard(AIBoard board, int nb_tries = 1)
        {
            _board = board;
            LaunchFramework(10, nb_tries, false);
        }

    }
}