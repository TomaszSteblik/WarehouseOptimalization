namespace Optimization.Helpers
{
    internal static class PointsArrayGenerator
    {
        public static int[] GeneratePointsToVisit(int size)
        {
            int[] result = new int[size];
            for (int i = 0; i < size; i++)
            {
                result[i] = i;
            }

            return result;
        }
    }
}