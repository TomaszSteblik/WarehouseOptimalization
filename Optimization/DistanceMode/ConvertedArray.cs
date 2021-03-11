namespace OptimizationMethods.DistanceMode
{
    public class ConvertedArray
    {
        private int[] _translation;
        private double[][] _convertedMatrix;

        public double[][] GetConvertedMatrix() => _convertedMatrix;

        public ConvertedArray(double[][] distancesMatrix, int[] order)
        {
            int size = order.Length;
            _convertedMatrix = new double[size][];
            _translation = new int[size];
            for (int i = 0; i < size; i++)
            {
                _convertedMatrix[i] = new double[size];
                _translation[i] = order[i];
            }

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    _convertedMatrix[i][j] = distancesMatrix[_translation[i]][_translation[j]];
                }
            }
            
        }
    }
}