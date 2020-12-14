using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Optimization
{
    public class Warehouse
    {
        public static void Optimizer(OptimizationParameters optimizationParameters)
        {
            
            //odczytać strukturę magazynu (mag.txt) i na jej podstawie wyznaczyć macierz odległości
            //double[][] warehouseStructure = OdczytPliku("mag.txt");
            //int[][] distances = Dijkstra.GenerateDistanceArray(warehouseStructure);
            //int[][] orders =  OdczytPliku("order.txt");

            //genereracja losowej populacji; każdy osobnik reprezentuje rozkład towarów

            //order.txt
            // 3 4 5 1 50
            // 4 5 1 

            //double[] FitnessProductPlacement;

            //AEX
            //for (int e = 0; e < maxEpoch; e++) //opt. rozkładu produktów
            //{

                //selekcja

                //krzyżowanie


                //fiteness
                //Parallel.For(0, liczbaOsobnikow_RoznychRozkladowMagazynu, i =>
                //{
                    
                 //   for (int k = 0; k < numberOfOrders; k++)
                  //  {
                  //      int dl_sciezki =  //znajdz najkrótsza ścieżke albo przez NN albo przez HGrex opcjonalnie z 2-opt albo Permutations.FindShortetsRoute();
                   //         FitnessProductPlacement[i] += dl_sciezki * ile_razy_bylo_takie_samo_zam;
                  //  }

               // });


                //mutacje 2        4

           // }

            //miejsca w magazynie    0  1  2  3  4  5  6
            //produkty              [0  3  4  1  5  6  2]
        }
    }
}