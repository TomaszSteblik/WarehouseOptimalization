using System;
using System.Collections.Generic;
using System.Linq;

namespace Optimization.GeneticAlgorithms.Crossovers
{

    internal class MPHGreXCrossover : Crossover
    {
        Random rnd = new Random();

        public MPHGreXCrossover(double[][] distancesMatrix) : base(distancesMatrix)
        {
        }

        protected int[] GenerateOffspring(int[][] parents)
        {
            int[] offspring = new int[parents[0].Length];

            for (int i = 0; i < offspring.Length; i++) //removing 0 from array for easier calculation
            {
                offspring[i] = -1;
            }



            int startPoint = 0;
            offspring[0] = parents[0][startPoint];



            int currentVertex = parents[0][startPoint];
            double currentWeight = 0;
            List<int> connectionsVertices = new List<int>();
            List<double> connectionsWeights = new List<double>();



            for (int cr = 1; cr < offspring.Length; cr++)
            {
                connectionsVertices.Clear();
                connectionsWeights.Clear();
                //searching for feasible connections in  parents
                for (int i = 0; i < parents.Count(); i++)
                {
                    for (int j = 0; j < offspring.Length; j++)
                    {
                        if (parents[i][j] == currentVertex)
                        {
                            if (j == offspring.Length - 1)
                            {
                                connectionsVertices.Add(parents[i][0]);
                                break;
                            }
                            else
                            {
                                connectionsVertices.Add(parents[i][j + 1]);
                                break;
                            }
                        }
                    }
                }



                //removing non feasible connections
                connectionsVertices = connectionsVertices.Except(offspring).ToList();



                if (connectionsVertices.Count() > 0)//calculating cost if feasible connections exist
                {
                    for (int i = 0; i < connectionsVertices.Count(); i++)
                    {
                        connectionsWeights.Add(DistancesMatrix[currentVertex][connectionsVertices[i]]);
                    }
                    currentWeight = connectionsWeights.Min();
                    currentVertex = connectionsVertices[connectionsWeights.IndexOf(currentWeight)];
                    offspring[cr] = currentVertex;
                }
                else //searching for other feasible path
                {
                    for (int i = 0; i < DistancesMatrix[0].Length; i++)
                    {
                        if (i == currentVertex || offspring.Contains(i))
                        {
                            connectionsWeights.Add(Double.MaxValue);
                        }
                        else
                        {
                            connectionsWeights.Add(DistancesMatrix[currentVertex][i]);
                        }
                    }
                    currentWeight = connectionsWeights.Min();
                    currentVertex = connectionsWeights.IndexOf(currentWeight);
                    offspring[cr] = currentVertex;
                }
            }



            return offspring;
        }
        

        public override int[][] GenerateOffsprings(int[][] parents, int numParentsForOneChild = 8)
        {
            var parentsLength = parents.Length;
            var amountOfChildren = parentsLength / 2;
            int[][] offsprings = new int[amountOfChildren][];

            for (int c = 0; c < amountOfChildren; c++)
            {
                int[][] prnt = new int[numParentsForOneChild][];
                for (int i = 0; i < numParentsForOneChild; i++)
                {
                    prnt[i] = parents[rnd.Next(parents.Length)];
                }

                offsprings[c] = GenerateOffspring(prnt);
            }

            return offsprings;
        }
        
        
    }
}
