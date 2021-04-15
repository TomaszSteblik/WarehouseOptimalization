using System;
using System.Collections.Generic;
using System.Linq;

namespace Optimization.GeneticAlgorithms.Selections
{
    internal class TournamentSelection : Selection {
        public int ParticipantsCount { get; set; }
        public override int[][] GenerateParents(int numberOfParents, double[] fitness)
        {
            if (ParticipantsCount < 2) ParticipantsCount = 2;
            var selected = new List<int>();

            for (int i = 0; i < numberOfParents; i++)
            {
                var participants = Enumerable.Range(0, PopulationSize)
                    .OrderBy(x => Random.Next())
                    .Take(ParticipantsCount);

                var indexToSelect = participants.Min();
                selected.Add(indexToSelect);
            }

            var parents = new int[numberOfParents][];
            for (int i = 0; i < numberOfParents; i++)
            {
                parents[i] = Population[selected[i]];
            }

            return parents;
        }

        public TournamentSelection(int[][] population, Random random) : base(population, random)
        {
        }
    }
}