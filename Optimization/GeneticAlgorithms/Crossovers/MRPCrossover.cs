using System.Collections.Generic;

namespace Optimization.GeneticAlgorithms.Crossovers
{
    class MRPCrossover : Crossover
    {
        private readonly List<Crossover> _crossovers;
        public MRPCrossover(CrossoverMethod[] crossoverMethods, int startPoint)
        {
            _crossovers = new List<Crossover>();
            foreach (var method in crossoverMethods)
            {
                _crossovers.Add(GeneticFactory.CreateCrossover(startPoint, method, null));
            }
        }

        public override int[] GenerateOffspring(int[][] parents)
        {
            return _crossovers[Random.Next(0,_crossovers.Count)].GenerateOffspring(parents);
        }
    }
}