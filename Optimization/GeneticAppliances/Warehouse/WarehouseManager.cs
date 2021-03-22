using System.IO;
using Optimization.Helpers;

namespace Optimization.GeneticAppliances.Warehouse
{
    internal class WarehouseManager
    {
        private double[][] _warehouseStructure;
        private int _warehouseSize;

        public int WarehouseSize => _warehouseSize;
        
        public double[][] CreateWarehouseDistancesMatrix(string warehouseSource)
        {
            double[][] distances; 
           
            _warehouseStructure = Files.ReadArray(warehouseSource);
            _warehouseSize = _warehouseStructure.GetLength(0);

            if (File.Exists(warehouseSource + ".dist.txt"))
            {
                distances = Files.ReadArray(warehouseSource + ".dist.txt");
            }
            else
            {
                distances = Dijkstra.GenerateDistanceArray(_warehouseStructure);
                Files.WriteArray(warehouseSource + ".dist.txt", distances);
            }

            return distances;

        }
    }
}