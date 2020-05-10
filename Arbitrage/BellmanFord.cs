using System;
using System.Collections.Generic;
using System.Linq;

namespace Arbitrage
{
    /// <summary>
    /// Total time complexity should be no more than O(|N|*|E|) or, rather, the number of nodes times the number of edges.
    /// </summary>
    public class BellmanFord
    {
        private Node[] Nodes { get; }

        public List<List<Node>> ProfitablePaths { get; } = new List<List<Node>>();

        public BellmanFord(Node[] nodes)
        {
            Nodes = nodes;
        }

        public void Calculate()
        {
            RelaxEdges();
            FindProfitablePaths();
        }

        /// <summary>
        /// This iterates through the Nodes N-1 times and discovers optimal distances over edges.
        /// This is also known as relaxing the edges. Adjusted distances are applied to the Node
        /// with respect to the starting node.
        /// </summary>
        private void RelaxEdges()
        {
            Nodes[0].Distance = 0; // Choose a random node to be the starting point

            for (int i = 0; i < Nodes.Length - 1; i++) // Iterate through Nodes: Nodes.Count - 1 times
            {
                foreach (var node in Nodes)
                {
                    foreach (var edge in node.Edges)
                    {
                        // If a node distance has not been calculated then skip
                        // and address on a following iteration when it is set.
                        if (node.Distance == double.PositiveInfinity) continue;

                        var distance = node.Distance + edge.Weight;

                        // If the distance of the edge's source node plus the weight of the edge is smaller than the
                        // current destination node's distance, then use the smaller distance as that indicates an
                        // improved path has been found.
                        if (distance < edge.DestinationNode.Distance)
                        {
                            edge.DestinationNode.Distance = distance;
                            edge.DestinationNode.OptimalPreviousNode = node;
                        }
                    }
                }
            }
        }

        private void FindProfitablePaths()
        {
            for (int source = 0; source < Nodes.Length; source++)
            {
                for (int destination = 0; destination < Nodes[source].Edges.Count; destination++)
                {
                    if (Nodes[destination].Distance > Nodes[source].Distance + Nodes[source].Edges[destination].Weight)
                    {
                        // Negative cycle exists
                        if (!Nodes[destination].Equals(Nodes[source]))
                        {
                            var optimalPath = new List<Node>(new[] { Nodes[destination], Nodes[source] });
                            while (!optimalPath.Contains(Nodes[source].OptimalPreviousNode))
                            {
                                optimalPath.Add(Nodes[source].OptimalPreviousNode);
                                source = Nodes.ToList().IndexOf(Nodes[source].OptimalPreviousNode);
                            }
                            optimalPath.Add(optimalPath[0]);
                            optimalPath.Reverse();
                            ProfitablePaths.Add(optimalPath);
                        }
                        
                    }
                }
            }
        }

        public void ShowProfitablePath(List<Node> profiablePath)
        {
            var path = string.Join(" --> ", profiablePath.Select(n => n.Name));
            Console.WriteLine(path);
            Console.WriteLine("------------------------------------------------------------------");
            return;
        }
    }
}
