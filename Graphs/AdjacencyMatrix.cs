using System.IO;
using System.Text;

namespace Graphs
{
    public class AdjacencyMatrix
    {
        private readonly bool[,] adjacencyMatrix;
        private readonly int numberOfVertices;

        public AdjacencyMatrix(int numberOfVertices)
        {
            this.numberOfVertices = numberOfVertices;
            this.adjacencyMatrix = new bool[numberOfVertices, numberOfVertices];
        }

        public void AddEdge(int from, int to)
        {
            this.adjacencyMatrix[from, to] = true;
            this.adjacencyMatrix[to, from] = true;
        }

        public void RemoveEdge(int from, int to)
        {
            this.adjacencyMatrix[from, to] = false;
            this.adjacencyMatrix[to, from] = false;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("  ");
            for (int i = 0; i < this.numberOfVertices; i++)
            {
                stringBuilder.Append($"{i}  ");
            }

            stringBuilder.AppendLine();

            for (int i = 0; i < this.numberOfVertices; i++)
            {
                stringBuilder.Append($"{i}:");
                for (int j = 0; j < this.numberOfVertices; j++)
                {
                    stringBuilder.Append(adjacencyMatrix[i, j] ? 1 : 0).Append("  ");
                }

                stringBuilder.AppendLine();
            }

            return stringBuilder.ToString();
        }
    }
}