using System.Linq;

namespace Optimization.GeneticAlgorithms.Modules
{
    public class TerminationModule : GeneticModule<double[]>
    {
        private double lastBest;
        private int epochCount;
        private int maxEpoch;
        public bool RequestedStop { get; private set; }
        
        
        public override string GetDesiredObject()
        {
            return "fitness";
        }

        public TerminationModule(int epochs)
        {
            lastBest = -1d;
            epochCount = 0;
            maxEpoch = epochs;
            Action = fitness =>
            {
                if (epochCount == maxEpoch) RequestedStop = true;

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