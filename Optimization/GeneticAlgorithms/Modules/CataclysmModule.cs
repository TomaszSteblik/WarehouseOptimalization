using System;
using System.Linq;

namespace Optimization.GeneticAlgorithms.Modules
{
    public class CataclysmModule : GeneticModule<int[][]>
    {
        private int epochCount;
        
        public CataclysmModule(Func<int[], int, int, int[][]> initializePopulation)
        {
            epochCount = 0;
            Action = population =>
            {
                if (epochCount++ != 200) return;

                epochCount = 0;
                int populationSize = population.Length;
                int eliminated = populationSize - populationSize / 10;

                int[][] newIndividuals = initializePopulation(population[0], populationSize, population[0][0]);
                var populationAfterCataclysm = population.Take(populationSize - eliminated).Concat(newIndividuals).ToArray();
                for (int i = 0; i < populationSize; i++)
                {
                    for (int j = 0; j < population[0].Length; j++)
                    {
                        population[i][j] = populationAfterCataclysm[i][j];
                    }
                }
            };
        }

        public override string GetDesiredObject()
        {
            return "population";
        }

    }
}