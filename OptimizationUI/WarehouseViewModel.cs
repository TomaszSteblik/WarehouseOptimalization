using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Optimization.Parameters;

namespace OptimizationUI
{
    public class WarehouseViewModel : WarehouseParameters, INotifyPropertyChanged
    {
        private string _warehousePath = "";
        public override string WarehousePath
        {
            get
            {
                return _warehousePath;
            }
            set
            {
                _warehousePath = value;
                NotifyPropertyChanged();
            }
        }

        private string _ordersPath = "";
        public override string OrdersPath
        {
            get
            {
                return _ordersPath;
            }
            set
            {
                _ordersPath = value;
                NotifyPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}