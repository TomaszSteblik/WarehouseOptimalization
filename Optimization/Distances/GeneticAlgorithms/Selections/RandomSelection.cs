namespace Optimization.Distances.GeneticAlgorithms.Selections
{
    internal class RandomSelection : Selection
    {

        public override int[][] GenerateParents(int numberOfParents, double[] fitness)
        {
             
            //losowo wybrac x z aktualnej populacji
            int[][] parents = new int[PopulationSize][];
            
           
            for (int k = 0; k < PopulationSize ; k++)
            {
                int index = Random.Next(0, PopulationSize);
                parents[k] = Population[index];
            }
            
            return parents;
        }

        public RandomSelection(int[][] population) : base(population)
        {
        }
    }
}