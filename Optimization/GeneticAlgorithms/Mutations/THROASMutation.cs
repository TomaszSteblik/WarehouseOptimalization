namespace Optimization.GeneticAlgorithms.Mutations
{
    internal class THROASMutation : Mutation
    {
        public THROASMutation(int[][] population, double mutationProbability) : base(population, mutationProbability)
        {
        }

        public override void Mutate()
        {
            if (_probability > 0d)
            {
                for (int m = (int)(0.1 * _population.Length); m < _population.Length; m++)
                {
                    if (_random.Next(0, 1000) <= _probability)
                    {
                        var pointA = _random.Next(1, _population[m].Length);
                        var pointB = _random.Next(pointA, _population[m].Length-1);
                        var pointC = _random.Next(pointB, _population[m].Length);

                        var valueA = _population[m][pointA];
                        var valueB = _population[m][pointB];
                        var valueC = _population[m][pointC];

                        _population[m][pointA] = valueC;
                        _population[m][pointC] = valueB;
                        _population[m][pointB] = valueA;

                    }
                }
            }
        }
    }
}