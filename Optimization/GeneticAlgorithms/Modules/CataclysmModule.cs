using System;
using System.Linq;
using Optimization.GeneticAlgorithms.Initialization;

namespace Optimization.GeneticAlgorithms.Modules
{
    internal class CataclysmModule : GeneticModule<int[][]>
    {
        private int epochCount;
        private int epochThreshold;
        
        public CataclysmModule(PopulationInitialization initialization, int percentage, int epochs)
        {
            epochCount = 0;
            epochThreshold = epochs;
            Action = population =>
            {
                if (epochCount++ != epochThreshold) return;

                epochCount = 0;
                int populationSize = population.Length;
                int eliminated = populationSize - populationSize * (100 - percentage) / 100;

                int[][] newIndividuals = initialization.InitializePopulation(population[0], populationSize, population[0][0]);
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