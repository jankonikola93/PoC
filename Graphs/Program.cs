using System;

namespace Graphs
{
    class Program
    {
        static void Main(string[] args)
        {
            var adjacencyMatrix = new AdjacencyMatrix(4);
            adjacencyMatrix.AddEdge(0, 1);
            adjacencyMatrix.AddEdge(0, 2);
            adjacencyMatrix.AddEdge(1, 2);
            adjacencyMatrix.AddEdge(2, 0);
            adjacencyMatrix.AddEdge(2, 3);

            Console.WriteLine(adjacencyMatrix.ToString());
        }
    }
}
