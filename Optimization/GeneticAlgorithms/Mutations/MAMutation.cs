using System.Collections.Generic;

namespace Optimization.GeneticAlgorithms.Mutations
{
    class MAMutation : Mutation
    {
        private List<Mutation> Mutations;
        private int _counter;
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
            if (_counter >= Mutations.Count) _counter = 0;
            Mutations[_counter].Mutate(chromosome);
            _counter++;
        }
    }
}