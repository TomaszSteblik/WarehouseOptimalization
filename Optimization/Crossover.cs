using System;
using System.Collections.Generic;
using System.Linq;

namespace Optimization
{
    public abstract class Crossover
    {
        protected abstract int[] GenerateOffspring(int[] parent1, int[] parent2);
        public abstract int[][] GenerateOffsprings(int[][] parents);
        protected readonly Random Random = new Random();

        protected bool IsThereGene(int[] chromosome, int a)
        {
            return chromosome.Any(t => t == a);
        }

        protected CityDistances _cityDistances = CityDistances.GetInstance();

        public class AexCrossover : Crossover
        {
            protected override int[] GenerateOffspring(int[] parent1, int[] parent2)
            {
                var length = parent1.Length;
                var offspring = new int[length];
                for (int i = 0; i < length; i++)
                {
                    offspring[i] = -1;
                }

                int[] currentParent;
                int[] nextParent;
                if (Random.Next(0, 1) == 1)
                {
                    currentParent = parent1;
                    nextParent = parent2;
                }
                else
                {
                    currentParent = parent2;
                    nextParent = parent1;
                }

                var count = -1;
                offspring[0] = currentParent[0];
                count++;
                offspring[1] = currentParent[1];
                count++;

                while (count < length - 1)
                {
                    int index = -1;
                    for (int i = 0; i < length; i++)
                    {
                        if (nextParent[i] == offspring[count])
                        {
                            var temp = currentParent;
                            currentParent = nextParent;
                            nextParent = temp;
                            index = i + 1;
                            break;
                        }
                    }

                    int alle = -1;
                    if (index < length)
                    {
                        if (!IsThereGene(offspring, currentParent[index]))
                        {
                            alle = currentParent[index];
                        }
                        else
                        {
                            while (IsThereGene(offspring, alle)) //losowac z niewykorzystanych
                            {
                                alle = Random.Next(0, length);
                            }
                        }
                    }
                    else
                    {
                        while (IsThereGene(offspring, alle))
                        {
                            alle = Random.Next(0, length);
                        }

                    }

                    count++;
                    offspring[count] = alle;
                }

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

        public class HGreXCrossover : Crossover
        {
            protected override int[] GenerateOffspring(int[] parent1, int[] parent2)
            {
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
                        distancePanent1 = _cityDistances._distances[currentAlle][nextAlleParent1];
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
                        distanceParent2 = _cityDistances._distances[currentAlle][nextAlleParent2];
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
                        case (isParent1Feasible: true, isParent2Feasible: true):

                            if (distancePanent1 < distanceParent2)
                            {
                                nextAlle = nextAlleParent1;
                            }
                            else
                            {
                                nextAlle = nextAlleParent2;
                            }

                            break;
                        case (isParent1Feasible: true, isParent2Feasible: false):

                            nextAlle = nextAlleParent1;

                            break;
                        case (isParent1Feasible: false, isParent2Feasible: true):

                            nextAlle = nextAlleParent2;

                            break;
                        case (isParent1Feasible: false, isParent2Feasible: false):

                            //generuj losowe i wez najlepszy

                            //int alle1, alle2;
                            //double distanceAlle1, distanceAlle2;

                            int lght = length - i;
                            int[] avaibleAlles = new int[lght];
                            int count = 0;
                            for (int j = 0; j < length; j++)
                            {
                                if (!IsThereGene(offspring, j))
                                {
                                    avaibleAlles[count] = j;
                                    count++;
                                }
                            }

                            nextAlle = avaibleAlles[Random.Next(0, lght)];

                            break;
                    }

                    offspring[i] = nextAlle;
                    currentAlle = nextAlle;
                }

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
}