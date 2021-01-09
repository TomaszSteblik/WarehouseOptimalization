using System.IO;

namespace Optimization
{
    public class WarehouseManager
    {
        private int _warehouseSize;
        private double[][] _warehouseStructure;
        private static WarehouseManager _instance;
        
        public int WarehouseSize => _instance._warehouseSize;

        public static WarehouseManager GetInstance() => _instance;
        
        private WarehouseManager(){}

        public static double[][] CreateWarehouse(string warehouseSource)
        {
            _instance ??= new WarehouseManager();
            double[][] distances; 
           
            _instance._warehouseStructure = Files.ReadArray(warehouseSource);
            _instance._warehouseSize = _instance._warehouseStructure.GetLength(0);

            if (File.Exists(warehouseSource + ".dist.txt"))
            {
                distances = Files.ReadArray(warehouseSource + ".dist.txt");
            }
            else
            {
                distances = Dijkstra.GenerateDistanceArray(_instance._warehouseStructure);
                Files.WriteArray(warehouseSource + ".dist.txt", distances);
            }

            return distances;

        }
    }
}