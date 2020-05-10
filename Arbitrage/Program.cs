using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Arbitrage
{
    class Program
    {
        static void Main(string[] args)
        {
            var dataFile = LoadData();
            var nodesAndEdges = BuildGraph(dataFile);
            ShowNodes(nodesAndEdges.Item1);
            ShowEdges(nodesAndEdges.Item2);
            var algorithm = new BellmanFord(nodesAndEdges.Item1, nodesAndEdges.Item2);
            algorithm.Calculate();
            algorithm.ShowProfitablePath();
            if (algorithm.ProfitablePath.Count > 0) TestResults(algorithm.ProfitablePath.ToArray(), nodesAndEdges.Item2);
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

        private static (Node[], Edge[]) BuildGraph(DataFile dataFile)
        {
            var nodes = new List<Node>();
            var edges = new List<Edge>();

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
                edges.Add(edge);
                var reverseEdge = new Edge(destinationNode, sourceNode, 1 / entry.Rate);
                edges.Add(reverseEdge);
            }

            return (nodes.ToArray(), edges.ToArray());
        }

        private static void ShowNodes(Node[] nodes)
        {
            Console.WriteLine();
            foreach (var node in nodes.OrderBy(n => n.Name).ToArray())
            {
                Console.WriteLine($"{node.Name}");
            }
            Console.WriteLine();
        }

        private static void ShowEdges(Edge[] edges)
        {
            Console.WriteLine();
            foreach (var edge in edges.OrderBy(e => e.SourceNode.Name).ToArray())
            {
                Console.WriteLine($"{edge.SourceNode.Name} ==> {edge.DestinationNode.Name}: {edge.Rate}");
            }
            Console.WriteLine();
        }

        //private static Node[] FindMostProfitablePath()
        //{
        //    var negLogOfRateMatrix = Nodes.Select(n => n.Edges.Select(e => -Math.Log(e.Rate)).ToArray()).ToArray();
        //    var matrixRowCount = negLogOfRateMatrix.Length;
        //    var minimumDistribution = Enumerable.Repeat(double.PositiveInfinity, matrixRowCount).ToArray();
        //    var initial = Enumerable.Repeat(-1, matrixRowCount).ToArray();
        //    minimumDistribution[0] = 0;

        //    // Calculate most profitable path
        //    for (int i=0; i<matrixRowCount-1; i++)
        //    {
        //        for (int fromCurrency=0; fromCurrency<matrixRowCount; fromCurrency++)
        //        {
        //            for (int toCurrency=0; toCurrency<matrixRowCount; toCurrency++)
        //            {
        //                if (minimumDistribution[toCurrency] > minimumDistribution[fromCurrency] + negLogOfRateMatrix[fromCurrency][toCurrency])
        //                {
        //                    minimumDistribution[toCurrency] = minimumDistribution[fromCurrency] + negLogOfRateMatrix[fromCurrency][toCurrency];
        //                    initial[toCurrency] = fromCurrency;
        //                }
        //            }
        //        }
        //    }

        //    // Show results
        //    var optimalPath = new List<int>();
        //    var currencyNames = Nodes.Select(n => n.Name);
        //    for (int fromCurrency=0; fromCurrency<matrixRowCount; fromCurrency++)
        //    {
        //        for (int toCurrency=0; toCurrency<matrixRowCount; toCurrency++)
        //        {
        //            if (minimumDistribution[toCurrency] > minimumDistribution[fromCurrency] + negLogOfRateMatrix[fromCurrency][toCurrency])
        //            {
        //                optimalPath.AddRange(new[] { toCurrency, fromCurrency });
        //                var tmpFromCurrency = fromCurrency;
        //                while (!optimalPath.Contains(initial[tmpFromCurrency]))
        //                {
        //                    optimalPath.Add(initial[tmpFromCurrency]);
        //                    tmpFromCurrency = initial[tmpFromCurrency];
        //                }
        //                optimalPath.Add(initial[tmpFromCurrency]);
        //            }
        //        }
        //    }

        //    var nodesInOrder = optimalPath.Select(i => Nodes[i]);
        //    return nodesInOrder.ToArray();
        //}

        //private static Node[] FindBestPath(Node[] nodes)
        //{
        //    var evaluations = Enumerable.Repeat(double.PositiveInfinity, nodes.Length).ToArray();
        //}

        //private static void ShowResults(Node[] optimalNodePath)
        //{
        //    if (optimalNodePath.Length > 0)
        //    {
        //        Console.WriteLine("Found an exchange path with a profit!");
        //        Console.WriteLine();
        //        var path = string.Join(" --> ", optimalNodePath.Select(n => n.Name));
        //        Console.WriteLine(path);
        //    }
        //    else
        //    {
        //        Console.WriteLine("No optimal path found.  :(");
        //    }
        //}

        private static void TestResults(Node[] path, Edge[] edges)
        {
            Console.Write($"Input a currency amount in {path[0].Name} (Default=1000): ");
            var moneyString = Console.ReadLine();
            double money;
            if (!double.TryParse(moneyString, out money)) money = 1000;

            var profitableEdges = new List<Edge>();
            for (int i=0; i<path.Length-1; i++)
            {
                var edge = edges.First(e => e.SourceNode.Name == path[i].Name && e.DestinationNode.Name == path[i + 1].Name);
                profitableEdges.Add(edge);
            }
            var returnEdge = edges.First(e => e.SourceNode.Name == path[path.Length - 1].Name && e.DestinationNode.Name == path[0].Name);
            profitableEdges.Add(returnEdge);

            double exchangedMoney = money;
            foreach (var edge in profitableEdges)
            {
                exchangedMoney *= edge.Rate;
            }
            Console.WriteLine($"If you exchanged {path[0].Name} {money} using this path, you would end up with {path[0].Name} {exchangedMoney}");
        }
    }
}
