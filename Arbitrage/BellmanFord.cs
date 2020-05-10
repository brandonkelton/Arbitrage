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

        public List<List<Node>> ProfitablePaths { get; } = new List<List<Node>>();

        public BellmanFord(Node[] nodes, Edge[] edges)
        {
            Nodes = nodes;
            Edges = edges;
        }

        public void Calculate()
        {
            Nodes[0].Distance = 0;

            for (int i = 0; i < Nodes.Length - 1; i++)
            {
                foreach (var node in Nodes)
                {
                    foreach (var edge in Edges.Where(e => e.SourceNode.Equals(node)))
                    {
                        // Using positive infinity to indicate a non-calculated path in source node and act as 
                        // a comparison against destination nodes to assist in determining the shortest path.
                        if (edge.SourceNode.Distance == double.PositiveInfinity) continue;

                        var distance = edge.SourceNode.Distance + edge.Weight;

                        // If the distance from the edge's source node plus the weight of the edge is smaller than the
                        // current destination node's distance from some other node, then use the smaller distance.
                        if (distance < edge.DestinationNode.Distance)
                        {
                            // Setting the destination node's cumulative distance (path traversal).
                            edge.DestinationNode.Distance = distance;
                            // Setting the optimal traversal between the two nodes as their distance between
                            // one another along the path is smaller than was previously set.
                            edge.DestinationNode.OptimalPathPreviousNode = edge.SourceNode;
                        }
                    }
                }
            }

            // Now get paths that provide for a profit
            //foreach (var node in Nodes)
            //{
            //    var currentNode = node;
            //    while (true)
            //    {
            //        var edge = Edges.Where(e => e.SourceNode.Equals(currentNode)).Min(e => e.Sour)

            //    }
            //}

            foreach (var edge in Edges)
            {
                var profitPath = new List<Node>();
                if (edge.SourceNode.Distance != double.PositiveInfinity && PathExists(edge))
                {
                    var node = edge.SourceNode;
                    while (true)
                    {
                        if (node.Equals(edge.DestinationNode)) break;
                        profitPath.Add(node);
                        node = node.OptimalPathPreviousNode;
                    }
                    //ProfitablePath.Add(edge.DestinationNode);
                    break;
                }
            }

            //foreach (var edge in Edges)
            //{
            //    var profitPath = new List<Node>();
            //    if (edge.SourceNode.Distance != double.PositiveInfinity && PathExists(edge))
            //    {
            //        var node = edge.SourceNode;
            //        while (true)
            //        {
            //            if (node.Equals(edge.DestinationNode)) break;
            //            profitPath.Add(node);
            //            node = node.OptimalPathPreviousNode;
            //        }
            //        //ProfitablePath.Add(edge.DestinationNode);
            //        break;
            //    }
            //}
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
