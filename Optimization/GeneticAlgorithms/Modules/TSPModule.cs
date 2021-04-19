using System.Collections.Generic;

namespace Optimization.GeneticAlgorithms.Modules
{
    public class TSPModule : GeneticModule<double[]>
    {
        private List<double[]> fitnessHistory;

        private List<int> resolveCountInEpoch;
        public override string GetDesiredObject()
        {
            return "fitness";
        }

        public double[][] GetFitnessHistory() => fitnessHistory.ToArray();

        public int[] ResolveCountInEpoch => resolveCountInEpoch.ToArray();

        public void AddResolveCount(int count)
        {
            resolveCountInEpoch.Add(count);
        }

        public TSPModule()
        {
            fitnessHistory = new List<double[]>();
            resolveCountInEpoch = new List<int>();
            
            Action = fitness =>
            {
                fitnessHistory.Add(new double[fitness.Length]);

                for (int i = 0; i < fitness.Length; i++)
                {
                    fitnessHistory[^1][i] = fitness[i];
                }
            };
        }

    }
}