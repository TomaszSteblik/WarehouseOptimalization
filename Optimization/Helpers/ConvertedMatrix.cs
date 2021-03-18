using System;
using System.Linq;

namespace Optimization.Helpers
{
    internal class ConvertedMatrix
    {
        private int[] _translation;
        private double[][] _convertedMatrix;

        public double[][] GetConvertedMatrix() => _convertedMatrix;
        public int[] GetTranslation() => _translation;

        public ConvertedMatrix(double[][] distancesMatrix, int[] order)
        {
            int size = order.Length;
            int[] temp = new int[size];
            _convertedMatrix = new double[size][];
            _translation = new int[size];
            for (int i = 0; i < size; i++)
            {
                _convertedMatrix[i] = new double[size];
            }

            temp = order.OrderBy(x => x).ToArray();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    _convertedMatrix[i][j] = distancesMatrix[temp[i]][temp[j]];
                }
            }

            _translation = new int[distancesMatrix.Length];
            for (int i = 0; i < distancesMatrix.Length; i++)
            {
                if (temp.Contains(i))
                {
                    _translation[i] = Array.IndexOf(temp, i);
                }
                else
                {
                    _translation[i] = -1;
                }
            }


        }
    }
}