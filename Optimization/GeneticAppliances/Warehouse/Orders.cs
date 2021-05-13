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

        public int HighestProductNumber;

        public int OrdersCount => _ordersCount;
        public int[][] OrdersList => orders;
        public int[] OrderRepeats => orderRepeats;

        public static Dictionary<int, int> ProductFrequencies;
        public static int[][] ProductsNamesInTheSameOrder;
        public static int[][] ProductsFrequenciesInTheSameOrder;

        public Orders(string ordersPath)
        {
            ProductFrequencies = new Dictionary<int, int>();
            ProductsNamesInTheSameOrder = null;
            ProductsFrequenciesInTheSameOrder = null;

            string[] fileLines = File.ReadAllLines(ordersPath);
            _ordersCount = fileLines.Length;
            orders = new int[_ordersCount][];
            orderRepeats = new int[_ordersCount];
            for (int i = 0; i < _ordersCount; i++)
            {
                int[] tmp = Array.ConvertAll(fileLines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries), int.Parse);
                orders[i] = new int[tmp.Length];
                orderRepeats[i] = tmp[tmp.Length - 1];
                orders[i][0] = 0; //warehouse entry is at position 0.

                for (int j = 1; j < tmp.Length; j++)
                {
                    orders[i][j] = tmp[j - 1];
                    if (ProductFrequencies.ContainsKey(tmp[j - 1]))
                        ProductFrequencies[tmp[j - 1]] += orderRepeats[i];
                    else
                        ProductFrequencies.Add(tmp[j - 1], orderRepeats[i]);
                }

            }

            GetProductCoexistence();
        }


        public void GetProductCoexistence()
        {
            HighestProductNumber = ProductFrequencies.Keys.Max();
            if (HighestProductNumber > 2 * ProductFrequencies.Count)
                throw new Exception("HighestProductNumber(" + HighestProductNumber + ") > ProductFrequencies.Count+1(" + 2 * ProductFrequencies.Count + ")");

            ProductsNamesInTheSameOrder = new int[HighestProductNumber + 1][];
            ProductsFrequenciesInTheSameOrder = new int[HighestProductNumber + 1][];
        

                foreach (KeyValuePair<int, int> product in ProductFrequencies)
                {
                    Dictionary<int, int> productsInTheSameOrderTmp = new Dictionary<int, int>();
                    for (int i = 0; i < _ordersCount; i++)
                    {

                        bool isContained = false;
                        for (int j = 0; j < orders[i].Length; j++)
                        {
                            if (product.Key == orders[i][j])
                            {
                                isContained = true;
                                if (!productsInTheSameOrderTmp.ContainsKey(orders[i][j]))
                                    productsInTheSameOrderTmp.Add(orders[i][j], 0);
                                break;
                            }
                        }
                        if (isContained)
                        {
                            for (int j = 0; j < orders[i].Length; j++)
                            {
                                if (product.Key != orders[i][j])
                                {
                                    if (productsInTheSameOrderTmp.ContainsKey(orders[i][j]))
                                        productsInTheSameOrderTmp[orders[i][j]] += orderRepeats[i];
                                    else
                                        productsInTheSameOrderTmp.Add(orders[i][j], orderRepeats[i]);
                                }
                            }

                        }
                    }


                    ProductsNamesInTheSameOrder[product.Key] = new int[productsInTheSameOrderTmp.Count];
                    ProductsFrequenciesInTheSameOrder[product.Key] = new int[productsInTheSameOrderTmp.Count];

                    int x2 = 0;
                    foreach (KeyValuePair<int, int> product2 in productsInTheSameOrderTmp)
                    {
                        ProductsNamesInTheSameOrder[product.Key][x2] = product2.Key;
                        ProductsFrequenciesInTheSameOrder[product.Key][x2] = product2.Value;
                        x2++;
                    }

                    Array.Sort(ProductsFrequenciesInTheSameOrder[product.Key], ProductsNamesInTheSameOrder[product.Key]);
                    Array.Reverse(ProductsFrequenciesInTheSameOrder[product.Key]);
                    Array.Reverse(ProductsNamesInTheSameOrder[product.Key]);
                }



        }

    }
}