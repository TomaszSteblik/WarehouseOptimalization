namespace Optimization.WarehouseMode
{
    public class Translator
    {
        public static int[] TranslateWithChromosome(int[] order, int[] chromosome)
        {
            int[] result = new int[order.Length - 1];
            for (int i = 0; i < order.Length - 1; i++)
            {
                result[i] = chromosome[order[i]];
            }

            return result;
        }
    }
}