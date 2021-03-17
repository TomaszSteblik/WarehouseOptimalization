using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//code from https://www.videlin.eu/2016/04/28/shortest-path-in-graph-dijkstras-algorithm-c-implementation/

namespace Optimization.Warehouse
{
    internal class Dijkstra
    {
        private static List<int> DijkstraAlgorithm(double[][] graph, int sourceNode, int destinationNode)
        {
            var n = graph.Length;
            
            var distance = new double[n];
            for (int i = 0; i < n; i++)
            {
                distance[i] = int.MaxValue;
            }
            
            distance[sourceNode] = 0;
            
            var used = new bool[n];
            var previous = new int?[n];

            while (true)
            {
                double minDistance = int.MaxValue;
                var minNode = 0;
                for (int i = 0; i < n; i++)
                {
                    if (!used[i] && minDistance > distance[i])
                    {
                        minDistance = distance[i];
                        minNode = i;
                    }
                }
                
                if (minDistance == int.MaxValue)
                {
                    break;
                }
                
                used[minNode] = true;
                
                for (int i = 0; i < n; i++)
                {
                    if (graph[minNode][i] > 0)
                    {
                        var shortestToMinNode = distance[minNode];
                        var distanceToNextNode = graph[minNode][i];
                        
                        double totalDistance = shortestToMinNode + distanceToNextNode;
                        
                        if (totalDistance < distance[i])
                        {
                            distance[i] = totalDistance;
                            previous[i] = minNode;
                        }
                    }
                }
            }
            
            if (distance[destinationNode] == int.MaxValue)
            {
                return null;
            }

            var path = new LinkedList<int>();
            int? currentNode = destinationNode;
            while (currentNode != null)
            {
                path.AddFirst(currentNode.Value);
                currentNode = previous[currentNode.Value];
            }
            return path.ToList();
        }

        private static double ReturnWeight(double[][] graph, List<int> path)
        {
            if (path == null)
            {
                return 0;
            }
            
            double pathLength = 0;
            for (int i = 0; i < path.Count - 1; i++)
            {
                pathLength += graph[path[i]][path[i + 1]];
            }
            return pathLength;
            
        }
        
        public static double[][] GenerateDistanceArray(double[][] warehouseStructure)
        {
            double[][] Distance = new double[warehouseStructure.Length][];
            Parallel.For(0, Distance.Length, i =>           
            {
                Distance[i] = new double[warehouseStructure[i].Length];               
                for (int j = 0; j < i; j++)
                    Distance[i][j] = ReturnWeight(warehouseStructure, DijkstraAlgorithm(warehouseStructure, i, j));
            });


            for (int i = 0; i < Distance.Length; i++)
            {
                Distance[i][i] = 0;
                for (int j = 0; j < i; j++)
                    Distance[j][i] = Distance[i][j];
            }
            return Distance;
        }
    }
}
