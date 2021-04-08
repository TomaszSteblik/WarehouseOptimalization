using System.Linq;

namespace Optimization.GeneticAlgorithms.Modules
{
    public class TerminationModule : GeneticModule<double[]>
    {
        private double lastBest;
        private int epochCount;
        
        
        public override string GetDesiredObject()
        {
            return "fitness";
        }

        public TerminationModule()
        {
            lastBest = -1d;
            epochCount = 0;
            Action = fitness =>
            {
                if (epochCount == 1000) throw new GeneticModuleExit();

                if (lastBest == -1)
                {
                    lastBest = fitness.Min();
                    return;
                }

                if (lastBest == fitness.Min())
                {
                    epochCount++;
                    return;
                }

                lastBest = fitness.Min();
                epochCount = 0;

            };
        }

    }
}