using System;

namespace Optimization.DistanceMode.GeneticAlgorithms.Crossovers
{
    public class HGreXCrossover : Crossover
        {
            public HGreXCrossover(double[][] distancesMatrix)
            {
                DistancesMatrix = distancesMatrix;
            }
            protected override int[] GenerateOffspring(int[] parent1, int[] parent2)
            {
                //Log.AddToLog($"HGreX Crossover:\nParent 1({Distances.CalculatePathLength(parent1)}): {string.Join(";", parent1)}");
                //Log.AddToLog($"Parent 2({Distances.CalculatePathLength(parent2)}): {string.Join(";", parent2)}");

                int length = parent1.Length;
                int[] offspring = new int[length];


                offspring[0] = parent1[0];
                int currentAlle = offspring[0];

                for (int i = 1; i < length; i++)
                {
                    offspring[i] = -1;
                }

                for (int i = 1; i < length; i++)
                {
                    int indexOfCurrentAlleParent1 = Array.IndexOf(parent1, currentAlle);
                    int indexOfCurrentAlleParent2 = Array.IndexOf(parent2, currentAlle);

                    int nextAlleParent1, nextAlleParent2;
                    double distancePanent1 = 0, distanceParent2 = 0;
                    bool isParent1Feasible, isParent2Feasible;


                    if (indexOfCurrentAlleParent1 + 1 < length)
                    {
                        nextAlleParent1 = parent1[indexOfCurrentAlleParent1 + 1];
                        distancePanent1 = DistancesMatrix[currentAlle][nextAlleParent1];
                        if (IsThereGene(offspring, nextAlleParent1))
                        {
                            isParent1Feasible = false;
                        }
                        else
                        {
                            isParent1Feasible = true;
                        }
                    }
                    else
                    {
                        nextAlleParent1 = 0;
                        isParent1Feasible = false;
                    }

                    if (indexOfCurrentAlleParent2 + 1 < length)
                    {
                        nextAlleParent2 = parent2[indexOfCurrentAlleParent2 + 1];
                        distanceParent2 = DistancesMatrix[currentAlle][nextAlleParent2];
                        if (IsThereGene(offspring, nextAlleParent2))
                        {
                            isParent2Feasible = false;
                        }
                        else
                        {
                            isParent2Feasible = true;
                        }
                    }
                    else
                    {
                        nextAlleParent2 = 0;
                        isParent2Feasible = false;
                    }


                    int nextAlle;

                    switch (isParent1Feasible, isParent2Feasible)
                    {
                        case (Item1: true, Item2: true):

                            if (distancePanent1 < distanceParent2)
                            {
                                nextAlle = nextAlleParent1;
                            }
                            else
                            {
                                nextAlle = nextAlleParent2;
                            }

                            break;
                        case (Item1: true, Item2: false):

                            nextAlle = nextAlleParent1;

                            break;
                        case (Item1: false, Item2: true):

                            nextAlle = nextAlleParent2;

                            break;
                        case (Item1: false, Item2: false):

                            //generuj losowe i wez najlepszy

                            //int alle1, alle2;
                            //double distanceAlle1, distanceAlle2;

                            int lght = length - i;
                            int[] avaibleAlles = new int[lght];
                            int count = 0;
                            for (int j = 0; j < length; j++)
                            {
                                if (!IsThereGene(offspring, parent1[j]))
                                {
                                    avaibleAlles[count] = parent1[j];
                                    count++;
                                }
                            }

                            nextAlle = avaibleAlles[Random.Next(0, lght)];

                            break;
                    }

                    offspring[i] = nextAlle;
                    currentAlle = nextAlle;
                }
                //Log.AddToLog($"Offspring({Distances.CalculatePathLength(offspring)}): {string.Join(";", offspring)} \n");
                return offspring;
            }

            public override int[][] GenerateOffsprings(int[][] parents)
            {
                var parentsLength = parents.Length;
                var amountOfChildren = parentsLength / 2;
                int[][] offsprings = new int[amountOfChildren][];

                for (int c = 0, j = 0; c < amountOfChildren; j += 2, c++)
                {
                    offsprings[c] = GenerateOffspring(parents[j], parents[j + 1]);
                }
                return offsprings;
            }
        }
}