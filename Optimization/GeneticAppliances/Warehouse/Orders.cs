using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Optimization.Helpers;

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

        [Description("How many times given product appears in all orders (calculation includes orders repetition)" +
                     " At 0 index is warehouse entrance, it should be set to 0")]
        public static int[] ProductFrequency { get; private set; }
        [Description("How many times given products pair appear in all orders (calculation includes orders repetition)." +
                     " At 0 index is warehouse entrance, it should be set to 0")]
        public static int[][] ProductsTogetherFrequency { get; private set; }

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
            
            
            //calculating frequency

            var distances = Distances.GetInstance().DistancesMatrix;
            var productsCount = distances.Length;
            
            ProductsTogetherFrequency = new int[productsCount][];
            ProductFrequency = new int[productsCount];
            for (var i = 1; i < productsCount; i++)
            {
                ProductFrequency[i] = orders.Sum(x => 
                    (x.Contains(i) ? 1 : 0)*orderRepeats[Array.IndexOf(orders,x)]);

                ProductsTogetherFrequency[i] = new int[productsCount];
                for (var j = 1; j < productsCount; j++)
                {
                    ProductsTogetherFrequency[i][j] = orders.Sum(x => 
                        ((x.Contains(i)&&x.Contains(j)) ? 1 : 0)*orderRepeats[Array.IndexOf(orders,x)]);
                }
            }
        }
    }
}