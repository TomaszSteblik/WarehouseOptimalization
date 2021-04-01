namespace Optimization.GeneticAlgorithms.Mutations
{
    class MRPMutation : Mutation
    {
        public MRPMutation(MutationMethod[] mutationMethods, double mutationProbability, int[][] population) : base(mutationProbability, population)
        {
            
        }

        protected override void Mutate(int[] chromosome)
        {
            throw new System.NotImplementedException();
        }
    }
}