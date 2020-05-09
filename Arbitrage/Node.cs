using System;
using System.Collections.Generic;
using System.Text;

namespace Arbitrage
{
    public class Node
    {
        public string Name { get; private set; }

        public List<Edge> Edges { get; } = new List<Edge>();

        public Node(string name)
        {
            Name = name;
        }
    }
}
