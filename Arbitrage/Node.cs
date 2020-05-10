using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Arbitrage
{
    public class Node : IEquatable<Node>
    {
        public string Name { get; private set; }

        public double Distance { get; set; } = double.PositiveInfinity;

        public Node OptimalPathPreviousNode { get; set; }

        public Node(string name)
        {
            Name = name;
        }

        public bool Equals([AllowNull] Node other)
        {
            return Name.Equals(other.Name);
        }
    }
}
