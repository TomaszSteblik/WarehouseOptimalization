using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Optimization
{
    public class Distances
    {

        private int _objectCount;
        private int _warehouseSize;
        private int _ordersCount;
        public double[][] _warehouseStructure;
        public double[][] _distances;
        public int[][] orders;
        public int[] orderRepeats;

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
           
            _instance._warehouseStructure = Files.ReadArray(warehouseSource);
            _instance._warehouseSize = _instance._warehouseStructure.GetLength(0);

            if (System.IO.File.Exists(warehouseSource + ".dist.txt"))
            {
                _instance._distances = Files.ReadArray(warehouseSource + ".dist.txt");
            }
            else
            {
                _instance._distances = Dijkstra.GenerateDistanceArray(_instance._warehouseStructure);
                Files.WriteArray(warehouseSource + ".dist.txt", _instance._distances);
            }


        }

        public static void LoadDistances(string dataSource)
        {
            _instance ??= new Distances();
            var fileLines = File.ReadAllLines(dataSource);
            _instance._objectCount = fileLines.GetLength(0);
            _instance._distances = new double[_instance._objectCount][];
            for (int i = 0; i < _instance._objectCount; i++)
            {
                _instance._distances[i] = Array.ConvertAll(fileLines[i].Split(" "
                    , StringSplitOptions.RemoveEmptyEntries), double.Parse);
            }
        }

        public static int[] GenerateObjectIdList(int size)
        {
            int[] result = new int[size];
            for (int i = 0; i < size; i++)
            {
                result[i] = i;
            }

            return result;
        }

        public static void LoadOrders(string ordersPath)
        {
            if (_instance != null)
            {
                var fileLines = File.ReadAllLines(ordersPath);
                _instance._ordersCount = fileLines.Length;
                _instance.orders = new int[_instance._ordersCount][];
                _instance.orderRepeats = new int[_instance._ordersCount];
                for (int i = 0; i < _instance._ordersCount; i++)
                {
                    int[] tmp = Array.ConvertAll(fileLines[i].Split(" "
                        , StringSplitOptions.RemoveEmptyEntries), int.Parse);
                    _instance.orders[i] = new int[tmp.Length + 1];
                    _instance.orders[i][0] = 0;
                    for (int j = 1; j < tmp.Length + 1; j++)
                    {
                        _instance.orders[i][j] = tmp[j - 1];
                    }
                }

                for (int i = 0; i < _instance._ordersCount; i++)
                {
                    List<int> tmp = _instance.orders[i].ToList();
                    _instance.orderRepeats[i] = tmp[^1];
                    tmp.RemoveAt(tmp.Count - 1);
                    tmp = tmp.Distinct().ToList();
                    _instance.orders[i] = tmp.ToArray();
                }
            }
        }
        
        public static double GetDistanceBetweenObjects(int firstId, int secondId)
        {
            return _instance._distances[firstId][secondId];
        }
        
        public static double CalculatePathLengthDouble(int[] path)
        {
            var sum = 0d;
            for (int i = 0; i < path.Length - 1; i++)
                sum += _instance._distances[path[i]][path[i + 1]];
            return sum;
        }

    }
}