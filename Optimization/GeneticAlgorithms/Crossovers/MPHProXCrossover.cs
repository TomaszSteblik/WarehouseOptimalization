namespace Optimization.GeneticAlgorithms.Crossovers
{
    class MPHProXCrossover : HProXCrossover
    {
        public MPHProXCrossover(double[][] distancesMatrix) : base(distancesMatrix)
        {
        }
        
        public override int[][] GenerateOffsprings(int[][] parents, int numParentsForOneChild = 8)
        {
            var parentsLength = parents.Length;
            var amountOfChildren = parentsLength / 2;
            int[][] offsprings = new int[amountOfChildren][];

            for (int c = 0; c < amountOfChildren; c++)
            {
                int[][] prnt = new int[numParentsForOneChild][];
                for (int i = 0; i < numParentsForOneChild; i++)
                {
                    prnt[i] = parents[Random.Next(parents.Length)];
                }

                offsprings[c] = GenerateOffspring(prnt);
            }

            return offsprings;
        }

    }
}