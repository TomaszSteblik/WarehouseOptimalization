using System;
using System.Collections.Generic;
using System.Text;

namespace Optimization.GeneticAlgorithms.Initialization
{
    abstract class PopulationInitialization
    {
        public abstract int[][] InitializePopulation(int[] pointsToInclude, int populationSize, int startingPoint);
       
    }



    public enum PopulationInitializationMethod
    {
        StandardPathInitialization,
        PreferedCloseDistancePathInitialization,
        UniformInitialization,
        NonUniformInitialization
    }
}
