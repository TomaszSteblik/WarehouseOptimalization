using System;
using System.Linq;

namespace Optimization.GeneticAlgorithms.Modules
{
    public class CataclysmModule : GeneticModule<int[][]>
    {
        private int epochCount;
        
        public CataclysmModule()
        {
            epochCount = 0;
            Action = population =>
            {
                if (epochCount++ != 200) return;

                epochCount = 0;
                int populationSize = population.Length;
                for (int i = populationSize / 10; i < populationSize; i++)
                {
                    var tmp = population[0].OrderBy(x => Guid.NewGuid()).ToArray();
                    for (int j = 0; j < population[i].Length; j++)
                    {
                        population[i][j] = tmp[j];
                        if (population[i][j] == 0) population[i][j] = population[i][0];
                    }

                    population[i][0] = 0;

                }

            };
        }

        public override string GetDesiredObject()
        {
            return "population";
        }

    }
}