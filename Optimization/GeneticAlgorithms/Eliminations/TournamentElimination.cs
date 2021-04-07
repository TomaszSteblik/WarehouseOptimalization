using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Optimization.Helpers;

namespace Optimization.GeneticAlgorithms.Eliminations
{
    internal class TournamentElimination : Elimination
    {
        public TournamentElimination(int[][] population) : base(population)
        {
        }

        public override void EliminateAndReplace(int[][] offsprings, double[] fitnessProductPlacement)
        {
            var offspringCount = offsprings.Length;
            var participantsCount = 4;
            var eliminated = new List<int>();

            for (int i = 0; i < offspringCount; i++)
            {
                var participants = Enumerable.Range(0, PopulationSize)
                    .Except(eliminated)
                    .OrderBy(x => Guid.NewGuid())
                    .Take(participantsCount);

                var indexToEliminate = participants.Max();
                eliminated.Add(indexToEliminate);
                
                Population[indexToEliminate] = offsprings[i];
                
            }
            
        }
    }
}