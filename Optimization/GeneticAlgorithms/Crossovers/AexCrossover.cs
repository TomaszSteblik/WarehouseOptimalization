namespace Optimization.GeneticAlgorithms.Crossovers
{
    internal class AexCrossover : Crossover
        {
            public AexCrossover(double[][] distancesMatrix, int startingPoint) : base(distancesMatrix, startingPoint)
            {
            }

            protected int[] GenerateOffspring(int[] parent1, int[] parent2)
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
                                alle = parent1[Random.Next(0, parent1.Length)];
                            }
                        }
                    }
                    else
                    {
                        while (IsThereGene(offspring, alle))
                        {
                            alle = parent1[Random.Next(0, parent1.Length)];
                        }

                    }

                    count++;
                    offspring[count] = alle;
                }

                return offspring;
            }

            public override int[][] GenerateOffsprings(int[][] parents, int numParentsForOneChild = 2)
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