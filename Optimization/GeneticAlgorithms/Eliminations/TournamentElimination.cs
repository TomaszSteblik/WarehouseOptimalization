using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Optimization.Helpers;

namespace Optimization.GeneticAlgorithms.Eliminations
{
    internal class TournamentElimination : Elimination
    {
        
        public int ParticipantsCount { get; set; } 
        public TournamentElimination(int[][] population, Random random) : base(population, random)
        {
        }

        public override void EliminateAndReplace(int[][] offsprings, double[] fitnessProductPlacement)
        {
            var offspringCount = offsprings.Length;
            if (ParticipantsCount < 2) ParticipantsCount = 2;
            var eliminated = new List<int>();

            for (int i = 0; i < offspringCount; i++)
            {
                var participants = Enumerable.Range(0, PopulationSize)
                    .Except(eliminated)
                    .OrderBy(x => Random.Next())
                    .Take(ParticipantsCount);

                var indexToEliminate = participants.Max();
                eliminated.Add(indexToEliminate);
                
                Population[indexToEliminate] = offsprings[i];
                
            }
            
        }
    }
}