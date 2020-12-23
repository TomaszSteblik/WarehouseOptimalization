using System;
using System.Linq;

namespace Optimization {
    public abstract class Selection
    {
        protected readonly int[][] Population;
        protected readonly int PopulationSize;
        protected readonly Random Random;
        protected int Strictness = 1;

        public Selection(int[][] population)
        {
            Population = population;
            PopulationSize = population.Length;
            Random = new Random();
        }
        public bool IncreaseStrictness(int numberOfChildren)
        {
            if (numberOfChildren*Math.Pow(2, Strictness + 1) <= PopulationSize)
            {
                Strictness++;
                return true;
            }
            return false;
        }

        public abstract int[][] GenerateParents(int numberOfParents, double[] fitness);
    }

    public class TournamentSelection : Selection {
        
        

        public override int[][] GenerateParents(int numberOfParents, double[] fitness)
        {
            var tournamentSize = numberOfParents*2*2;
            //losowo wybrac x z aktualnej populacji
            int[][] contenders = new int[tournamentSize][];
            double[] tournamentFitness = new double[tournamentSize];
            //wypelnic
            for (int k = 0; k < tournamentSize; k++)
            {
                var index = Random.Next(0, PopulationSize);
                contenders[k] = Population[index];
                tournamentFitness[k] = fitness[index];
            }
            //rozegrac az nie bedzie jeden
            int[][] winner = Tournament(contenders,ref tournamentFitness);
            while (winner.Length>numberOfParents)
            {
                winner = Tournament(winner,ref tournamentFitness);
            }
            return winner;
        }

        private int[][] Tournament(int[][] contenders, ref double[] tournamentFitness)
        {
            //int lenght = contenders.Length * (int) Math.Pow(2,Strictness);
            int halfLenght = contenders.Length / 2;
            int[][] winners = new int[halfLenght][];
            double[] newFitness = new double[halfLenght];
            for (int i = 0,c=0; i < halfLenght; i++,c+=2) 
            {
                if (tournamentFitness[c]<tournamentFitness[c+1])
                {
                    winners[i] = contenders[c];
                    newFitness[i] = tournamentFitness[c];
                }
                else
                {
                    winners[i] = contenders[c + 1];
                    newFitness[i] = tournamentFitness[c + 1];
                }
            }

            tournamentFitness = newFitness;
            return winners;
        }
        


        public TournamentSelection(int[][] population) : base(population)
        {
        }
    }

    public class ElitismSelection : Selection
    {
        public ElitismSelection(int[][] population) : base(population)
        {
            
        }

        public override int[][] GenerateParents(int numberOfParents, double[] fitness)
        {
            Array.Sort(fitness,Population);
            
            int[][] parents = new int[numberOfParents][];

            for (int i = 0; i < numberOfParents; i++)
            {
                parents[i] = (int[]) Population[i].Clone();
            }

            
            
            return parents;
        }
    }

    public class RouletteWheelSelection : Selection
    {
        public RouletteWheelSelection(int[][] population) : base(population)
        {
            
        }

        public override int[][] GenerateParents(int numberOfParents, double[] fitness)
        {
            int[][] parents = new int[numberOfParents][];
            for (int i = 0; i < numberOfParents; i++)
            {
                parents[i] = GenerateSingleParent(fitness);
            }

            return parents;
        }

        private int[] GenerateSingleParent(double[] fitness)
        {
            double fitnessSum = fitness.Sum();
            
            for (int i = 0; i < PopulationSize; i++)
            {
                fitnessSum += (1.0 / fitness[i]);
                if (fitnessSum >= 1)
                {
                    return Population[i];
                }
            }

            return Population[PopulationSize - 1];
        }
        
    }
}