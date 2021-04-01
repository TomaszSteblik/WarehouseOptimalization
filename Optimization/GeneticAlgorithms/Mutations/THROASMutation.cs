namespace Optimization.GeneticAlgorithms.Mutations
{
    internal class THROASMutation : Mutation
    {
        public THROASMutation() : base()
        {
        }

        public override void Mutate(int[] chromosome)
        {
            var pointA = _random.Next(1, chromosome.Length-2);
            var pointB = _random.Next(pointA+1, chromosome.Length-1);
            var pointC = _random.Next(pointB+1, chromosome.Length);

            var valueA = chromosome[pointA];
            var valueB = chromosome[pointB];
            var valueC = chromosome[pointC];

            chromosome[pointA] = valueC;
            chromosome[pointC] = valueB;
            chromosome[pointB] = valueA;
        }
    }
}