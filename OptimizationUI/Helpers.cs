using System.Linq;

namespace OptimizationUI
{
    public static class Helpers
    {
        public static double[][] GetBestFitnesses(double[][][] runFitnesses)
        {

            int runs = runFitnesses.Length;
            int epoch = runFitnesses[0].Length;

            var expandedFitness = GetExpandedFitesses(runFitnesses);

            double[][] fitness = new double[runs][];

            for (int i = 0; i < runs; i++)
            {
                fitness[i] = new double[runFitnesses[0].Length];
            }

            for (int i = 0; i < runs; i++)
            {
                for (int j = 0; j < epoch; j++)
                {
                    fitness[i][j] = expandedFitness[i][j].Min();
                }
            }


            return fitness;
        }

        public static double[][][] GetExpandedFitesses(double[][][] runFitnesses)
        {
            double[][][] expandedFitness = new double[runFitnesses.Length][][];

            int[] lengths = new int[runFitnesses.Length];
            for (int i = 0; i < runFitnesses.Length; i++)
            {
                lengths[i] = runFitnesses[i].Length;
            }
            int epoch = lengths.Max();

            for (int i = 0; i < runFitnesses.Length; i++)
            {
                expandedFitness[i] = new double[epoch][];
            }

            for (int j = 0; j < runFitnesses.Length; j++)
            {
                for (int i = 0; i < epoch; i++)
                {
                    expandedFitness[j][i] = new double[runFitnesses[0][0].Length];
                }
            }

            for (int i = 0; i < expandedFitness.Length; i++)
            {
                for (int j = 0; j < expandedFitness[0].Length; j++)
                {
                    for (int k = 0; k < expandedFitness[0][0].Length; k++)
                    {
                        if (j >= lengths[i])
                        {
                            expandedFitness[i][j][k] = runFitnesses[i][lengths[i] - 1][k];
                        }
                        else
                        {
                            expandedFitness[i][j][k] = runFitnesses[i][j][k];
                        }
                    }
                }
            }

            return expandedFitness;
        }

        public static double[][] GetAverageFitnesses(double[][][] runFitnesses)
        {

            int epoch = runFitnesses[0].Length;

            var expandedFitness = GetExpandedFitesses(runFitnesses);

            double[][] fitness = new double[epoch][];

            for (int i = 0; i < epoch; i++)
            {
                fitness[i] = new double[runFitnesses[0][0].Length];
            }


            for (int i = 0; i < epoch; i++)
            {
                for (int j = 0; j < runFitnesses[0][0].Length; j++)
                {
                    fitness[i][j] = expandedFitness.Average(x => x[i][j]);
                }
            }

            return fitness;
        }
    }
}