using System;
using System.Collections.Generic;
using System.Linq;
using Optimization.GeneticAppliances.TSP;

namespace OptimizationUI
{
    public static class Logger
    {
        public static string CreateDistanceLogsPerRunsParams(TSPResult[] results, string conflictResolver, string randomResolver)
        {
            var fitness = Helpers.GetAverageFitnesses(results.Select(x => x.fitness).ToArray());
            var differences = Helpers.GetAverageFitnesses(results.Select(x => x.DifferencesInEpoch).ToArray());
            var z = results.Select(x => x.ResolvePercentInEpoch).ToArray();

            var maxLenght = z.Max(x => x.Length);
            var transponsedZ = new List<List<double>>();
            for (int i = 0; i < maxLenght; i++)
            {
                transponsedZ.Add(new List<double>());
                for (int j = 0; j < z.Length; j++)
                {
                    if (z[j].Length > i) transponsedZ[i].Add(z[j][i]);
                }
            }

            var epochsPerc = transponsedZ.Select(x => x.Average()).ToArray();

            string s = "";

            for (int i = 0; i < fitness.Length; i++)
            {
                var epochFitnesses = fitness[i];
                var difference = differences[i];
                s += i + ";";
                //tego nie jestem do końca pewien, które wartości będą potrzebne - może lepiej ich naprodukować więcej, by mieć z czego wybierać:
                s += epochFitnesses.Min().ToString("#.000") + ";";  //najlepszy wynik
                s += epochFitnesses.OrderBy(x => x).Take((int)(0.1 * epochFitnesses.Length)).Average().ToString("#.000") + ";"; //średnia z najlepszych 10%
                s += epochFitnesses.OrderBy(x => x).Skip((int)(0.5 * epochFitnesses.Length)).Take(1).Average().ToString("#.000") + ";";  //mediana
                s += epochFitnesses.OrderBy(x => x).Skip((int)(0.9 * epochFitnesses.Length)).Take((int)(0.1 * epochFitnesses.Length)).Average().ToString("#.000") + ";"; //średnia z najgorszych 10%
                s += epochFitnesses.Average().ToString("#.000") + ";"; //średnia
                s += epochFitnesses.Max().ToString("#.000") + ";"; // najgorszy wynik
                s += epochFitnesses.StandardDeviation().ToString("#.000") + ";"; // odchylenie standardowe
                s += epochsPerc[i].ToString("#.000") + ";";
                s += difference.Average().ToString("#.000") + ";";
                s += difference.Count(x => x == 0).ToString("#.000") + ";"; //identyczne
                s += difference.Count(x => x < Math.Max(2.1, 0.021 * results[0].BestGene.Length)).ToString("#.000"); //2% różnicy
                s += "\r\n";

            }

            return s;
        }
        
        public static string CreateDistanceLogsBestPerRunsParams(TSPResult[] results, string conflictResolver, string randomResolver)
        {
            var fitness = Helpers.GetBestFitnesses(results.Select(x => x.fitness).ToArray());
            var differences = Helpers.GetAverageFitnesses(results.Select(x => x.DifferencesInEpoch).ToArray());
            var z = results.Select(x => x.ResolvePercentInEpoch).ToArray();

            var maxLenght = z.Max(x => x.Length);
            var transponsedZ = new List<List<double>>();
            for (int i = 0; i < maxLenght; i++)
            {
                transponsedZ.Add(new List<double>());
                for (int j = 0; j < z.Length; j++)
                {
                    if (z[j].Length > i) transponsedZ[i].Add(z[j][i]);
                }
            }

            var epochsPerc = transponsedZ.Select(x => x.Average()).ToArray();

            string s = "";

            for (int i = 0; i < fitness[0].Length; i++)
            {
                var difference = differences[i];
                s += i + ";";
                //tego nie jestem do końca pewien, które wartości będą potrzebne - może lepiej ich naprodukować więcej, by mieć z czego wybierać:
                s += fitness.Min(x => x[i]).ToString("#.000") + ";";  //najlepszy wynik
                s += fitness.OrderBy(x => x[i]).Take((int)(0.1 * fitness.Length)).Average(x => x[i]).ToString("#.000") + ";"; //średnia z najlepszych 10%
                s += fitness.OrderBy(x => x[i]).Skip((int)(0.5 * fitness.Length)).Take(1).Average(x => x[i]).ToString("#.000") + ";";  //mediana
                s += fitness.OrderBy(x => x[i]).Skip((int)(0.9 * fitness.Length)).Take((int)(0.1 * fitness.Length)).Average(x => x[i]).ToString("#.000") + ";"; //średnia z najgorszych 10%
                s += fitness.Average(x => x[i]).ToString("#.000") + ";"; //średnia
                s += fitness.Max(x => x[i]).ToString("#.000") + ";"; // najgorszy wynik
                s += fitness.StandardDeviation(x => x[i]).ToString("#.000") + ";"; // odchylenie standardowe
                s += epochsPerc[i].ToString("#.000") + ";";
                s += difference.Average().ToString("#.000") + ";";
                s += difference.Count(x => x == 0).ToString("#.000") + ";"; //identyczne
                s += difference.Count(x => x < Math.Max(2.1, 0.021 * results[0].BestGene.Length)).ToString("#.000"); //2% różnicy
                s += "\n";

            }

            return s;
        }
    }
}