using System.Collections.Generic;
using System.Linq;
using Optimization.GeneticAlgorithms.Crossovers;

namespace Optimization.GeneticAlgorithms.Modules
{
    public class TSPModule : GeneticModule<double[]>
    {
        private List<double[]> fitnessHistory;

        private List<int> resolveCountInEpoch;
        private List<int> randomizedResolveCountInEpoch;
        private List<double> randomResolvePercents;
        private List<double> conflictResolvesPercents;
        private Crossover _crossover;
        public List<double[]> DifferencesInEpoch { get; set; }
        public override string GetDesiredObject()
        {
            return "fitness";
        }

        public void LoadCrossoverOperator(Crossover crossover)
        {
            _crossover = crossover;
        }
        public double[][] GetFitnessHistory() => fitnessHistory.ToArray();

        public int[] ResolveCountInEpoch => resolveCountInEpoch.ToArray();
        public int[] RandomizedResolveCountInEpoch => randomizedResolveCountInEpoch.ToArray();

        public double[] RandomResolvesPercent => randomResolvePercents.ToArray();
        public double[] ConflictResolvesPercent => conflictResolvesPercents.ToArray();

        public void AddResolveCount(int count)
        {
            resolveCountInEpoch.Add(count);
        }
        
        public void AddRandomResolvesPercent(double percent)
        {
            randomResolvePercents.Add(percent);
        }
        public void AddConflictResolvesPercent(double percent)
        {
            conflictResolvesPercents.Add(percent);
        }


        public void AddRandomizedResolveCount(int count)
        {
            randomizedResolveCountInEpoch.Add(count);
        }

        public TSPModule()
        {
            fitnessHistory = new List<double[]>();
            resolveCountInEpoch = new List<int>();
            randomizedResolveCountInEpoch = new List<int>();
            conflictResolvesPercents = new List<double>();
            randomResolvePercents = new List<double>();
            DifferencesInEpoch = new List<double[]>();
            
            Action = fitness =>
            {
                fitnessHistory.Add(new double[fitness.Length]);

                for (int i = 0; i < fitness.Length; i++)
                {
                    fitnessHistory[^1][i] = fitness[i];
                }
                AddResolveCount(_crossover.ResolveCount);
                AddRandomizedResolveCount(_crossover.RandomizedResolvesCount);
                AddConflictResolvesPercent(100.0 * _crossover.ResolveCount / _crossover.RandomizationChances);
                AddRandomResolvesPercent(100.0 * _crossover.RandomizedResolvesCount / _crossover.RandomizationChances);
                DifferencesInEpoch.Add(_crossover.difference);
            };
        }

    }
}