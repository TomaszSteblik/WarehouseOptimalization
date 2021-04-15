using System;
using System.IO;
using System.Text;

namespace Optimization.Helpers
{
    internal class Files
    {
        public static double[][] ReadArray(string fileName)
        {
            string extension = fileName.Split('.')[1].ToLower();
            switch (extension)
            {
                case "txt":
                    return ReadTxt(fileName);
                case "tsp":
                    return ReadTsp(fileName);
                default:
                    throw new ArgumentException("File format not supported");
            }
            

        }
        
        public static int[][] ReadArrayInt(string fileName)
        {

            string[] lines = File.ReadAllLines(fileName);
            int nonEmptyLines = 0;
            for (int i = 0; i < lines.Length; i++)
                if (lines[i].Trim().Length > 1)
                    nonEmptyLines++;

            int[][] Matrix = new int[nonEmptyLines][];
            for (int i = 0; i < nonEmptyLines; i++)
            {
                string[] s = lines[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                Matrix[i] = Array.ConvertAll(s, int.Parse);
            }

            return Matrix;

        }


        public static void WriteArray(string fileName, double[][] Matrix)
        {

            StringBuilder stringBuilder = new StringBuilder();

            for (int i = 0; i < Matrix.Length - 1; i++)
                stringBuilder.AppendLine(string.Join(" ", Matrix[i]));
            stringBuilder.Append(string.Join(" ", Matrix[Matrix.Length - 1]));

            File.WriteAllText(fileName, stringBuilder.ToString());

        }

        private static double[][] ReadTxt(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            int nonEmptyLines = 0;
            for (int i = 0; i < lines.Length; i++)
                if (lines[i].Trim().Length > 1)
                    nonEmptyLines++;

            double[][] Matrix = new double[nonEmptyLines][];
            for (int i = 0; i < nonEmptyLines; i++)
            {
                string[] s = lines[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                Matrix[i] = Array.ConvertAll(s, double.Parse);
            }

            return Matrix;
        }
        
        private static double[][] ReadTsp(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);
            int nonEmptyLines = 0;
            for (int i = 0; i < lines.Length; i++)
                if (lines[i].Trim().Length > 1 && lines[i]!="EOF")
                    nonEmptyLines++;

            int linesBeforeValues = 0;
            for (int i = 0; i < nonEmptyLines; i++)
            {
                if(int.TryParse(lines[i].Split(" ")[0],out var n))
                    break;
                linesBeforeValues++;
            }

            double[][] matrix = new double[nonEmptyLines-linesBeforeValues][];
            for (int i = 0; i < matrix.Length; i++)
            {
                matrix[i] = new double[matrix.Length];
            }
            double[] x = new double[nonEmptyLines-linesBeforeValues];
            double[] y = new double[nonEmptyLines-linesBeforeValues];

            for (int i = linesBeforeValues; i < nonEmptyLines; i++)
            {
                string[] s = lines[i].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                x[i-linesBeforeValues] = double.Parse(s[1].Replace('.',','));
                y[i-linesBeforeValues] = double.Parse(s[2].Replace('.',','));
            }

            for (int i = 0; i < x.Length; i++)
            {
                for (int j = 0; j < x.Length; j++)
                {
                    matrix[i][j] = Math.Round(Math.Sqrt((x[i] - x[j]) * (x[i] - x[j]) + (y[i] - y[j]) * (y[i] - y[j])),0);
                }
            }

            return matrix;
        }
    }
}