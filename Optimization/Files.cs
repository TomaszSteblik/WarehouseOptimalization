using System;
using System.IO;
using System.Text;

namespace Optimization
{
    internal class Files
    {
        public static double[][] ReadArray(string fileName)
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
    }
}