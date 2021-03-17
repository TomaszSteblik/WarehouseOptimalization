namespace Optimization.Distances.GeneticAlgorithms.Selections
{
    internal class TournamentSelection : Selection {
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
}