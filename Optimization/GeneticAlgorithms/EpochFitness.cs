using System;
using System.IO;
using System.Text;

namespace Optimization.GeneticAlgorithms
{
    internal class EpochFitness
    {
        private string _path;

        public EpochFitness(string path)
        {
            _path = path;
            File.Delete(_path);
        }
        public void AddLine(double best, double worst, double avg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{best},{worst},{Convert.ToInt32(avg)}{Environment.NewLine}");
            File.AppendAllText(_path, sb.ToString());
        }

        public void AddLine(double[] fitness)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var f in fitness)
            {
                sb.Append($"{f},");
            }

            sb.Append(Environment.NewLine);
            File.AppendAllText(_path, sb.ToString());
        }
    }
}