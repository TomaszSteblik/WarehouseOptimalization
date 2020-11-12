using System;

namespace GeneticAlgorithm {
    public abstract class Selection
    {
        public abstract int[][] GenerateParents(int numberOfParents);
        protected int[][] Population;
        protected int[][] Cities;
        protected int NumberOfCities;
        protected int PopulationSize;
        protected Random Random;
        public int Strictness = 1;

        public Selection(int[][] population)
        {
            Population = population;
            Cities = GeneticAlgorithm.Source.Data;
            NumberOfCities = Cities.Length;
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
    }

    public class TournamentSelection : Selection {
        
        
        public override int[][] GenerateParents(int numberOfParents)
        {
            var tournamentSize = numberOfParents*2*2;
            //losowo wybrac x z aktualnej populacji
            int[][] contenders = new int[tournamentSize][];
            //wypelnic
            for (int k = 0; k < tournamentSize; k++)
            {
                var index = Random.Next(0, PopulationSize);
                contenders[k] = Population[index];
            }
            //rozegrac az nie bedzie jeden
            int[][] winner = Tournament(contenders);
            while (winner.Length>numberOfParents)
            {
                winner = Tournament(winner);
            }
            return winner;
        }
        private int[][] Tournament(int[][] contenders)
        {
            int lenght = contenders.Length * (int) Math.Pow(2,Strictness);
            int halfLenght = contenders.Length / 2;
            int[][] winners = new int[halfLenght][];
            //Array.Sort(contenders,(x,y)=>Helper.Fitness(x)-Helper.Fitness(y));
            for (int i = 0,c=0; i < halfLenght; i++,c+=2) 
            {
                if (Helper.Fitness(contenders[c])<Helper.Fitness(contenders[c+1]))
                {
                    winners[i] = contenders[c];
                }
                else
                {
                    winners[i] = contenders[c + 1];
                }
            }
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
        public override int[][] GenerateParents(int numberOfParents)
        {
            Array.Sort(Population,(x,y)=>Helper.Fitness(x)-Helper.Fitness(y));
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

        public override int[][] GenerateParents(int numberOfParents)
        {
            int[][] parents = new int[numberOfParents][];
            for (int i = 0; i < numberOfParents; i++)
            {
                parents[i] = GenerateSingleParent();
            }

            return parents;
        }

        private int[] GenerateSingleParent()
        {
            double fitnessSum = 0;
            foreach (var gene in Population)
            {
                fitnessSum += Helper.Fitness(gene);
            }

            for (int i = 0; i < PopulationSize; i++)
            {
                fitnessSum += (1.0 / Helper.Fitness(Population[i]));
                if (fitnessSum >= 1)
                {
                    return Population[i];
                }
            }

            return Population[PopulationSize - 1];
        }
    }
}