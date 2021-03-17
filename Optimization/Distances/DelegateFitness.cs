namespace Optimization.Distances
{
    public static class DelegateFitness
    {
        public delegate double[] CalcFitness(int[][] population, double[][] distancesMatrix);
    }
}