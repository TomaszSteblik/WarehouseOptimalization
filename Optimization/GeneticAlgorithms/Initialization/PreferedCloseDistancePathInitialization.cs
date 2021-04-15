using System.Linq;
using System;
using Optimization.Helpers;
using System.IO;
using System.Collections.Generic;

namespace Optimization.GeneticAlgorithms.Initialization
{
    class PreferedCloseDistancePathInitialization : PopulationInitialization
    {
        

        bool log = false;
        public override int[][] InitializePopulation(int[] pointsToInclude, int populationSize, int startingPoint)
        {
            double[][] distanceMatrix = Distances.GetInstance().DistancesMatrix;


            int numCandidates = (int)(0.25 * distanceMatrix.Length);
            
            int[][] population = new int[populationSize][];

            try
            {
                //need to log this
                
                for (int i = 0; i < populationSize; i++)  //for each individual
                {
                    population[i] = new int[pointsToInclude.Length];
                    population[i][0] = 0;
                    
                    List<int> RemainingPoints = new List<int>(distanceMatrix.Length);
                    for (int j = 0; j < population[i].Length; j++)
                        RemainingPoints.Add(j);

                    int lastPoint = 0;

                    for (int j = 1; j < population[i].Length; j++) //for each position j in the individual
                    {
                                                
                        //Tournament Selection
                        double minDistance = Double.MaxValue;
                        int bestCandidate = -1;


                        for (int k = 0; k < numCandidates; k++)
                        {
                            int candidate = Random.Next(1, RemainingPoints.Count);
                            if (distanceMatrix[lastPoint][RemainingPoints[candidate]] < minDistance)
                            {
                                minDistance = distanceMatrix[lastPoint][RemainingPoints[candidate]];
                                bestCandidate = RemainingPoints[candidate];
                            }
                        }


                        population[i][j] = bestCandidate;
                        lastPoint = bestCandidate;
                        RemainingPoints.Remove(bestCandidate);

                    }


                    

                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                s += " ";
            }

            if (log)
            {
                string sp = "";
                double sumD = 0;
                for (int p = 0; p < population.Length; p++)
                {
                    double dist = 0;
                    for (int c = 0; c < population[0].Length; c++)
                    {
                        sp += population[p][c] + " ";
                        if (c > 0)
                            dist += distanceMatrix[population[p][c - 1]][population[p][c]];
                    }
                    dist += distanceMatrix[population[p][population[0].Length - 1]][population[p][0]];
                    sp += "   " + dist + "\r\n";
                    sumD += dist;
                }
                sp += sumD;
                File.WriteAllText(@"pd.txt", sp);
            }

            return population;

        }


        public PreferedCloseDistancePathInitialization(Random random) : base(random)
        {
        }
    }
}