using System;
using System.Collections.Generic;
using System.Text;

namespace Arbitrage
{
    public class Edge
    {
        public double Rate { get; private set; }

        public Node DestinationNode { get; private set; }

        public Edge(double rate, Node destinationNode)
        {
            Rate = rate;
            DestinationNode = destinationNode;
        }
    }
}
