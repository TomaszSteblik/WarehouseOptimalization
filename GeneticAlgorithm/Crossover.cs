using System;
using System.Collections.Generic;

namespace GeneticAlgorithm
{
    public abstract class Crossover
    {
        protected abstract int[] GenerateOffspring(int[] parent1, int[] parent2);
        public abstract int[][] GenerateOffsprings(int[][] parents);
        protected readonly Random Random = new Random();
    }

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
            
            while (count < length-1)
            {
                int index = -1;
                for (int i = 0; i < length; i++)
                {
                    if (nextParent[i] == offspring[count])
                    {
                        var temp = currentParent;
                        currentParent = nextParent;
                        nextParent = temp;
                        index = i+1;
                        break;
                    }
                }
                int alle = -1;
                if (index < length)
                {
                    if (!Helper.IsThereGene(offspring,currentParent[index]))
                    {
                        alle = currentParent[index];
                    }
                    else
                    {
                        while (Helper.IsThereGene(offspring,alle)) //losowac z niewykorzystanych
                        {
                            alle = Random.Next(0, length);
                        }
                    }
                }
                else
                {
                    while (Helper.IsThereGene(offspring,alle))
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
            
            for (int c=0,j = 0; c < amountOfChildren; j+=2,c++)
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
            var length = parent1.Length;
            var offspring = new int[length];
            int[] firstParent = parent1;
            int[] secondParent = parent2;

            if (Random.Next(0, 1) == 1)
            {
                offspring[0] = firstParent[0];
                offspring[1] = firstParent[1];
            }
            else
            {
                offspring[0] = secondParent[0];
                offspring[1] = secondParent[1];
            }

            var count = 2;
            var currentAlle = offspring[count];

            while (count<length)
            {

                int firstParentCurrentAlleIndex = Array.IndexOf(firstParent,currentAlle);
                int secondParentCurrentAlleIndex = Array.IndexOf(secondParent, currentAlle);


                double firstDistance;
                double secondDistance;
                if (firstParentCurrentAlleIndex + 1 < length)
                {
                    firstDistance = Helper.GetDistance(currentAlle, firstParentCurrentAlleIndex + 1);
                }
                else
                {
                    firstDistance = -1;
                }

                if (secondParentCurrentAlleIndex + 1 < length)
                {
                    secondDistance = Helper.GetDistance(currentAlle, secondParentCurrentAlleIndex + 1);
                }
                else
                {
                    secondDistance = -1;
                }

                 

                int nextAlle;
                
                if (firstDistance > secondDistance)
                {
                    nextAlle = firstParent[firstParentCurrentAlleIndex + 1];
                }
                else if(secondDistance > firstDistance)
                {
                    nextAlle = secondParent[secondParentCurrentAlleIndex + 1];
                }
                else
                {
                    nextAlle = GenerateNewAlle(length, offspring);
                }
                
                offspring[count] = nextAlle;
                currentAlle = nextAlle;
                count++;
            }
            

            
            return offspring;
        }

        private int GenerateNewAlle(int length,int[] offspring)
        {
            while (true)
            {
                int index = Random.Next(0, length);
                if (!Helper.IsThereGene(offspring, index))
                {
                    return index;
                }
            }
        }

        public override int[][] GenerateOffsprings(int[][] parents)
        {
            var parentsLength = parents.Length;
            var amountOfChildren = parentsLength / 2;
            int[][] offsprings = new int[amountOfChildren][];
            
            for (int c=0,j = 0; c < amountOfChildren; j+=2,c++)
            {
                offsprings[c] = GenerateOffspring(parents[j], parents[j + 1]);
            }
            return offsprings;
        }
    }
}