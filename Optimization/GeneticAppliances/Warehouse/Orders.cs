using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Optimization.GeneticAppliances.Warehouse
{
    internal class Orders
    {
        private int[][] orders;
        private int[] orderRepeats;
        private int _ordersCount;
        
        public int OrdersCount => _ordersCount;
        public int[][] OrdersList => orders;
        public int[] OrderRepeats => orderRepeats;

        public Orders(string ordersPath)
        {
            
            var fileLines = File.ReadAllLines(ordersPath);
            _ordersCount = fileLines.Length;
            orders = new int[_ordersCount][];
            orderRepeats = new int[_ordersCount];
            for (int i = 0; i < _ordersCount; i++)
            {
                int[] tmp = Array.ConvertAll(fileLines[i].Split(" "
                    , StringSplitOptions.RemoveEmptyEntries), int.Parse);
                orders[i] = new int[tmp.Length + 1];
                orders[i][0] = 0;
                for (int j = 1; j < tmp.Length + 1; j++)
                { 
                    orders[i][j] = tmp[j - 1];
                }
            }

            for (int i = 0; i < _ordersCount; i++)
            { 
                List<int> tmp = orders[i].ToList(); 
                orderRepeats[i] = tmp[^1]; 
                tmp.RemoveAt(tmp.Count - 1); 
                tmp = tmp.Distinct().ToList(); 
                orders[i] = tmp.ToArray();
            }
        }
    }
}