using System;
using System.IO;

namespace Optimization
{
    public class Distances
    {

        private int _objectCount;
        private int _warehouseSize;
        private int _ordersCount;
        public int[][] _distances;
        public double[][] _warehouseStructure;
        public double[][] _warehouseDistances;
        public int[][] orders;

        public static int ObjectCount
        {
            get => _instance._objectCount;
            private set => _instance._objectCount = value;
        }

        public int OrdersCount
        {
            get => _instance._ordersCount;
            private set => _instance._ordersCount = value;
        }

        public static int WarehouseSize
        {
            get => _instance._warehouseSize;
            private set => _instance._warehouseSize = value;
        }

        private static Distances _instance;

        private Distances()
        {
            _objectCount = 0;
        }

        public static Distances GetInstance()
        {
            return _instance;
        }

        public static void CreateWarehouse(string warehouseSource)
        {
            _instance ??= new Distances();
            var warehouse = File.ReadAllLines(warehouseSource);
            _instance._warehouseSize = warehouse.GetLength(0);
            _instance._warehouseStructure = new double[_instance._warehouseSize][];
            for (int i = 0; i < _instance._warehouseSize; i++)
            {
                _instance._warehouseStructure[i] = Array.ConvertAll(warehouse[i].Split(" "
                    , StringSplitOptions.RemoveEmptyEntries), double.Parse);
            }

            _instance._warehouseDistances = Dijkstra.GenerateDistanceArray(_instance._warehouseStructure);
        }

        public static void LoadDistances(string dataSource)
        {
            _instance ??= new Distances();
            var fileLines = File.ReadAllLines(dataSource);
            _instance._objectCount = fileLines.GetLength(0);
            _instance._distances = new int[_instance._objectCount][];
            for (int i = 0; i < _instance._objectCount; i++)
            {
                _instance._distances[i] = Array.ConvertAll(fileLines[i].Split(" "
                    , StringSplitOptions.RemoveEmptyEntries), int.Parse);
            }
        }

        public static void LoadOrders(string ordersPath)
        {
            if (_instance != null)
            {
                var fileLines = File.ReadAllLines(ordersPath);
                _instance._ordersCount = fileLines.Length;
                _instance.orders = new int[_instance._ordersCount][];
                for (int i = 0; i < _instance._ordersCount; i++)
                    _instance.orders[i] = Array.ConvertAll(fileLines[i].Split(" "
                        , StringSplitOptions.RemoveEmptyEntries), int.Parse);
            }
        }
        
        public static int GetDistanceBetweenObjects(int firstId, int secondId)
        {
            return _instance._distances[firstId][secondId];
        }

        public static int CalculatePathLength(int[] path)
        {
            var sum = 0;
            for (int i = 0; i < path.Length - 1; i++)
                sum += GetDistanceBetweenObjects(path[i], path[i + 1]);
            return sum;
        }
        
        public static double CalculatePathLengthDouble(int[] path)
        {
            var sum = 0;
            for (int i = 0; i < path.Length - 1; i++)
                sum += GetDistanceBetweenObjects(path[i], path[i + 1]);
            return sum;
        }
    }
}