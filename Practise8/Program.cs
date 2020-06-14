using System;
using System.Collections.Generic;

namespace Практика_Задание_8_11
{
    class Практика_Задание_8_11
    {
        public static Random random = new Random();
        static void Main()
        {
            Console.WriteLine("Задание 8:\n" +
                "Граф задан матрицей инциденций. Найти все его мосты.\n\n" +
                "----------");
            Console.Write("Задайте кол-во вершин: ");
            int vertex = int.Parse(Console.ReadLine());
            //Console.Write("Задайте кол-во ребер: ");
            //int edge = int.Parse(Console.ReadLine());
            int[,] Matrix = CreateMatrix(vertex, vertex);
            PrintMatrix(Matrix);

            Graph graph = new Graph(vertex);
            graph.AddEdges(Matrix);
            graph.Bridge();

            Console.ReadKey();
        }
        //ребра         //вершины
        public static int[,] CreateMatrix(int graphEdges, int graphVertexes)
        {
            int[,] newMatrix = new int[graphEdges, graphVertexes];
            //заполнить матицу нулями
            for (int i = 0; i < graphEdges; i++)
            {
                for (int j = 0; j < graphVertexes; j++)
                {
                    newMatrix[i, j] = 0;
                }
            }
            //добавить для каждого ребра две вершины
            for (int i = 0; i < graphEdges; i++)
            {
                int count = 2;
                do
                {
                    int vertex = random.Next(0, graphVertexes);
                    if (newMatrix[vertex, i] != 1)
                    {
                        newMatrix[vertex, i] = 1;
                        count--;
                    }
                } while (count > 0);
            }
            return newMatrix;
        }
        public static void PrintMatrix(int[,] Matrix)
        {
            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                for (int j = 0; j < Matrix.GetLength(1); j++)
                    if (Matrix[i, j] == 1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(String.Format("{0,3}", Matrix[i, j]));
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write(String.Format("{0,3}", Matrix[i, j]));
                    }
                Console.WriteLine();
            }
        }
        public class Graph
        {
            private readonly int V; // No. of vertices  

            // Array of lists for Adjacency List Representation  
            private readonly List<int>[] adjacency;
            int time = 0;
            static readonly int NIL = -1;

            // Constructor  
            public Graph(int v)
            {
                V = v;
                adjacency = new List<int>[v];
                for (int i = 0; i < v; ++i)
                    adjacency[i] = new List<int>();
            }

            // Function to add an edge into the graph  
            public void AddEdges(int[,] matrix)
            {
                int fistNode = -1;
                int secondNode = -1;
                //перебор столбцов
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    //перебор строк
                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (matrix[j, i] == 1)
                        {
                            if (fistNode == -1)
                                fistNode = j;
                            else
                                secondNode = j;
                        }
                    }
                    adjacency[fistNode].Add(secondNode);
                    try
                    {
                        adjacency[secondNode].Add(fistNode);
                    }
                    catch (System.IndexOutOfRangeException)
                    {
                        Console.WriteLine("Произошла ошибка при создании матрицы. Попробуйте ввод еще раз.");
                        Main();
                    }
                    fistNode = -1;
                    secondNode = -1;
                }
            }

            // A recursive function that finds and prints bridges  
            // using DFS traversal  
            // u --> The vertex to be visited next  
            // visited[] --> keeps tract of visited vertices  
            // disc[] --> Stores discovery times of visited vertices  
            // parent[] --> Stores parent vertices in DFS tree  
            void BridgeUtil(int u, bool[] visited, int[] disc,
                            int[] low, int[] parent)
            {

                // Mark the current node as visited  
                visited[u] = true;

                // Initialize discovery time and low value  
                disc[u] = low[u] = ++time;

                // Go through all vertices aadjacent to this  
                foreach (int i in adjacency[u])
                {
                    int v = i; // v is current adjacent of u  

                    // If v is not visited yet, then make it a child  
                    // of u in DFS tree and recur for it.  
                    // If v is not visited yet, then recur for it  
                    if (!visited[v])
                    {
                        parent[v] = u;
                        BridgeUtil(v, visited, disc, low, parent);

                        // Check if the subtree rooted with v has a  
                        // connection to one of the ancestors of u  
                        low[u] = Math.Min(low[u], low[v]);

                        // If the lowest vertex reachable from subtree  
                        // under v is below u in DFS tree, then u-v is  
                        // a bridge  
                        if (low[v] > disc[u])
                            Console.WriteLine(u + " " + v);
                    }

                    // Update low value of u for parent function calls.  
                    else if (v != parent[u])
                        low[u] = Math.Min(low[u], disc[v]);
                }
            }


            // DFS based function to find all bridges. It uses recursive  
            // function bridgeUtil()  
            public void Bridge()
            {
                // Mark all the vertices as not visited  
                bool[] visited = new bool[V];
                int[] disc = new int[V];
                int[] low = new int[V];
                int[] parent = new int[V];


                // Initialize parent and visited,   
                // and ap(articulation point) arrays  
                for (int i = 0; i < V; i++)
                {
                    parent[i] = NIL;
                    visited[i] = false;
                }

                // Call the recursive helper function to find Bridges  
                // in DFS tree rooted with vertex 'i'  
                for (int i = 0; i < V; i++)
                    if (visited[i] == false)
                        BridgeUtil(i, visited, disc, low, parent);
            }
        }
    }
}
