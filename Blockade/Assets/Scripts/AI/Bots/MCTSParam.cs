using blockade.AI.Bots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blockade.AI.Bots
{
    public struct MCTSParam
    {
        public string configName;
        public int maxThinkTime; // in miliseconds
        public int maxIterations;
        public double explorationCoef;
    }
}
