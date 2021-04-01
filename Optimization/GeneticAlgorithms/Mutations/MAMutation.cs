using System.Collections.Generic;

namespace Optimization.GeneticAlgorithms.Mutations
{
    class MAMutation : Mutation
    {
        private List<Mutation> Mutations;
        private int counter = 0;
        public MAMutation(MutationMethod[] mutationMethods, double mutationProbability, int[][] population) : base(mutationProbability, population)
        {
            Mutations = new List<Mutation>();
            foreach (var method in mutationMethods)
            {
                Mutations.Add(GeneticFactory.CreateMutation(method,null,population,mutationProbability));
            }
        }

        public override void Mutate(int[] chromosome)
        {
            if (counter >= 4) counter = 0;
            Mutations[counter].Mutate(chromosome);
            counter++;
        }
    }
}