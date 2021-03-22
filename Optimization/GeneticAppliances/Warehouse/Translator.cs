namespace Optimization.GeneticAppliances.Warehouse
{
    internal class Translator
    {
        public static int[] TranslateWithChromosome(int[] order, int[] chromosome)
        {
            int[] result = new int[order.Length];
            for (int i = 0; i < order.Length; i++)
                for (int c = 0; c < chromosome.Length; c++)
                    if (order[i] == chromosome[c])
                    {
                        result[i] = c;
                        break;
                    }
            return result;
        }
    }
}