using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arbitrage
{
    public class BellmanFord
    {
        private Node[] Nodes { get; }

        private Edge[] Edges { get; }

        public List<Node> ProfitablePath { get; } = new List<Node>();

        public BellmanFord(Node[] nodes, Edge[] edges)
        {
            Nodes = nodes;
            Edges = edges;
        }

        public void Calculate()
        {
            Nodes[0].Distance = 0;

            foreach (var node in Nodes)
            {
                foreach (var edge in Edges.Where(e => e.SourceNode.Equals(node)))
                {
                    if (edge.SourceNode.Distance == double.PositiveInfinity) continue;
                    var distance = edge.SourceNode.Distance + edge.Weight;

                    if (distance < edge.DestinationNode.Distance)
                    {
                        edge.DestinationNode.Distance = distance;
                        edge.DestinationNode.OptimalPathPreviousNode = edge.SourceNode;
                    }
                }
            }

            foreach (var edge in Edges)
            {
                if (edge.SourceNode.Distance != double.PositiveInfinity && PathExists(edge))
                {
                    var count = 0;
                    var node = edge.SourceNode;
                    while (true)
                    {
                        if (node.Equals(edge.DestinationNode)) break;
                        count++;
                        if (count > 30) break;
                        ProfitablePath.Add(node);
                        node = node.OptimalPathPreviousNode;
                    }
                    ProfitablePath.Add(edge.DestinationNode);
                    break;
                }
            }
        }

        private bool PathExists(Edge edge)
        {
            return edge.DestinationNode.Distance > edge.SourceNode.Distance + edge.Weight;
        }

        public void ShowProfitablePath()
        {
            if (ProfitablePath.Count > 0)
            {
                var path = string.Join(" --> ", ProfitablePath.Select(n => n.Name));
                Console.WriteLine(path);
                return;
            }

            Console.WriteLine("No profitable path exists.");
        }
    }
}
