using System;
using System.ComponentModel;
using Optimization.Parameters;

namespace AvaloniaUI.Models
{
    public class Warehouse : WarehouseParameters, INotifyPropertyChanged
    {
        
        public override string WarehousePath { get; set; } = "";
        public override string OrdersPath { get; set; } = "";
        
        
        public event PropertyChangedEventHandler PropertyChanged;
       
    }
}