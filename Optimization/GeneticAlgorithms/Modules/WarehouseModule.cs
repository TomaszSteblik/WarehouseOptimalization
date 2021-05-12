using System.Collections.Generic;

namespace Optimization.GeneticAlgorithms.Modules
{
    public class WarehouseModule : GeneticModule<double[]>
    {
        private List<double[]> fitnessHistory;
        
        public override string GetDesiredObject()
        {
            return "fitness";
        }
        public double[][] GetFitnessHistory() => fitnessHistory.ToArray();
        
        public WarehouseModule()
        {
            fitnessHistory = new List<double[]>();
            
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