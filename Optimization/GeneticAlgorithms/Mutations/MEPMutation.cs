namespace Optimization.GeneticAlgorithms.Mutations
{
    class MEPMutation : Mutation
    {
        public MEPMutation(MutationMethod[] mutationMethods, double mutationProbability, int[][] population) : base(mutationProbability, population)
        {
            
        }

        protected override void Mutate(int[] chromosome)
        {
            throw new System.NotImplementedException();
        }
    }
}