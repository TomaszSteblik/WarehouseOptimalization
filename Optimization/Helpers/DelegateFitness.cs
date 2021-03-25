namespace Optimization.Helpers
{
    public static class DelegateFitness
    {
        public delegate double[] CalcFitness(int[][] population);
    }
}