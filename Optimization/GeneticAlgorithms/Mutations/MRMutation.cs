using System.Collections.Generic;

namespace Optimization.GeneticAlgorithms.Mutations
{
    class MRMutation : Mutation
    {
        private List<Mutation> Mutations;
        public MRMutation(MutationMethod[] mutationMethods, double mutationProbability, int[][] population) : base(mutationProbability, population)
        {
            Mutations = new List<Mutation>();
            foreach (var method in mutationMethods)
            {
                Mutations.Add(GeneticFactory.CreateMutation(method,null,population,mutationProbability));
            }
        }

        public override void Mutate(int[] chromosome)
        {
            Mutations[Random.Next(0,Mutations.Count)].Mutate(chromosome);
        }
    }
}