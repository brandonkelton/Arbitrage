using System;
using System.Collections.Generic;
using System.Linq;

namespace Arbitrage
{
    class Program
    {
        private static DataFile DataFile;
        private static readonly List<Node> Nodes = new List<Node>();

        static void Main(string[] args)
        {
            LoadData();
            BuildGraph();
            FindMostProfitablePath();
        }

        private static void LoadData()
        {
            DataFile = new DataFile("Data.txt");
            DataFile.LoadFile().Wait();
            Console.WriteLine($"Data file loaded with {DataFile.Entries.Count} entries.");
        }

        private static void BuildGraph()
        {
            var entryEnumerator = DataFile.Entries.GetEnumerator();
            while (entryEnumerator.MoveNext())
            {
                var entry = entryEnumerator.Current;
                var fromNode = Nodes.FirstOrDefault(n => n.Name == entry.FromCountry);
                var toNode = Nodes.FirstOrDefault(n => n.Name == entry.ToCountry);
                if (fromNode == null)
                {
                    fromNode = new Node(entry.FromCountry);
                    Nodes.Add(fromNode);
                }
                if (toNode == null)
                {
                    toNode = new Node(entry.ToCountry);
                    Nodes.Add(toNode);
                }
                var edge = new Edge(entry.Rate, toNode);
                fromNode.Edges.Add(edge);
                var returnEdge = new Edge(1 / entry.Rate, fromNode);
                toNode.Edges.Add(returnEdge);
            }
        }

        private static void FindMostProfitablePath()
        {
            var rateMatrix = Nodes.Select(n => n.Edges.Select(e => e.Rate).ToArray()).ToArray();
            var negLogOfRateMatrix = Nodes.Select(n => n.Edges.Select(e => -Math.Log(e.Rate)).ToArray()).ToArray();

            var matrixRowCount = negLogOfRateMatrix.Length;
            var minimumDistribution = Enumerable.Repeat(double.PositiveInfinity, matrixRowCount).ToArray();
            var pre = Enumerable.Repeat(-1, matrixRowCount).ToArray();
            minimumDistribution[0] = 0;

        }
    }
}
