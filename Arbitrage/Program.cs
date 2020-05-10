using System;
using System.Collections.Generic;
using System.Linq;

namespace Arbitrage
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataFile = LoadData();
            var nodes = BuildGraph(dataFile);
            ShowNodes(nodes);
            ShowEdges(nodes);
            var algorithm = new BellmanFord(nodes);
            algorithm.Calculate();
            
            if (algorithm.ProfitablePaths.Count > 0)
            {
                Console.WriteLine("================");
                Console.WriteLine("Profitable Paths");
                Console.WriteLine("================");
                algorithm.ProfitablePaths.ForEach(path => algorithm.ShowProfitablePath(path));
                TestResults(algorithm.ProfitablePaths);
            }
            else
            {
                Console.WriteLine("No profitable path exists");
            }
            
            Console.ReadLine();
        }

        private static DataFile LoadData()
        {
            var dataFile = new DataFile("Data.txt");
            dataFile.LoadFile();
            Console.WriteLine();
            Console.WriteLine($"Data file loaded with {dataFile.Entries.Count} entries.");
            Console.WriteLine();
            return dataFile;
        }

        private static Node[] BuildGraph(DataFile dataFile)
        {
            var nodes = new List<Node>();

            var entryEnumerator = dataFile.Entries.GetEnumerator();
            while (entryEnumerator.MoveNext())
            {
                var entry = entryEnumerator.Current;
                var sourceNode = nodes.FirstOrDefault(n => n.Name == entry.FromCountry);
                var destinationNode = nodes.FirstOrDefault(n => n.Name == entry.ToCountry);
                if (sourceNode == null)
                {
                    sourceNode = new Node(entry.FromCountry);
                    nodes.Add(sourceNode);
                }
                if (destinationNode == null)
                {
                    destinationNode = new Node(entry.ToCountry);
                    nodes.Add(destinationNode);
                }
                var edge = new Edge(sourceNode, destinationNode, entry.Rate);
                sourceNode.Edges.Add(edge);
                var reverseEdge = new Edge(destinationNode, sourceNode, 1 / entry.Rate);
                destinationNode.Edges.Add(reverseEdge);
            }

            return nodes.ToArray();
        }

        private static void ShowNodes(Node[] nodes)
        {
            Console.WriteLine();
            Console.WriteLine("Available Nodes");
            Console.WriteLine("---------------");
            foreach (var node in nodes.OrderBy(n => n.Name).ToArray())
            {
                Console.WriteLine($"{node.Name}");
            }
            Console.WriteLine();
        }

        private static void ShowEdges(Node[] nodes)
        {
            Console.WriteLine();
            Console.WriteLine("Available Edges");
            Console.WriteLine("---------------");
            foreach (var edge in nodes.SelectMany(n => n.Edges).OrderBy(e => e.SourceNode.Name).ToArray())
            {
                Console.WriteLine($"{edge.SourceNode.Name} ==> {edge.DestinationNode.Name}: {edge.Rate}");
            }
            Console.WriteLine();
        }

        private static void TestResults(List<List<Node>> paths)
        {
            Console.WriteLine();

            foreach (var nodes in paths)
            {
                Console.WriteLine();
                Console.Write($"Input a currency amount in {nodes[0].Name} (Default=1000): ");
                var moneyString = Console.ReadLine();
                double money;
                if (!double.TryParse(moneyString, out money)) money = 1000;

                var profitableEdges = new List<Edge>();
                for (int i = 0; i < nodes.Count - 1; i++)
                {
                    var edge = nodes[i].Edges.First(e => e.DestinationNode.Name == nodes[i + 1].Name);
                    profitableEdges.Add(edge);
                }

                double exchangedMoney = money;
                foreach (var edge in profitableEdges)
                {
                    exchangedMoney *= edge.Rate;
                }
                Console.WriteLine($"If you exchanged {nodes[0].Name} {money} using this path, you would end up with {nodes[0].Name} {exchangedMoney}");
            }
            
        }
    }
}
