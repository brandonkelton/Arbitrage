﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Arbitrage
{
    public class Edge
    {
        public Node SourceNode { get; private set; }

        public Node DestinationNode { get; private set; }

        public double Rate { get; private set; }

        public double Weight { get; private set; }

        public Edge(Node sourceNode, Node destinationNode, double rate)
        {
            SourceNode = sourceNode;
            DestinationNode = destinationNode;
            Rate = rate;
            Weight = -1 * Math.Log(rate);
        }
    }
}
