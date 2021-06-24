using System;
using System.ComponentModel;
using Optimization.Parameters;

namespace OptimizationUI.Models
{
    public class Warehouse : WarehouseParameters, INotifyPropertyChanged
    {
        
        public override string WarehousePath { get; set; } = "";
        public override string OrdersPath { get; set; } = "";

        public override OptimizationParameters FitnessGeneticAlgorithmParameters { get; set; }
        public override OptimizationParameters WarehouseGeneticAlgorithmParameters { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        public Warehouse()
        {
        }
       
    }
}