using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Optimization.Parameters;

namespace OptimizationUI
{
    public class WarehouseViewModel : WarehouseParameters, INotifyPropertyChanged
    {
        
        private bool _showCustom = true;

        public bool ShowCustom
        {
            get => _showCustom;
            set
            {
                _showCustom = value;
                NotifyPropertyChanged();
            }
        }
        

        private bool _showBest = true;

        public bool ShowBest
        {
            get => _showBest;
            set

            {
                _showBest = value;
                NotifyPropertyChanged();
            }
        }
        private bool _showAvg = true;

        public bool ShowAvg
        {
            get => _showAvg;
            set

            {
                _showAvg = value;
                NotifyPropertyChanged();
            }
        }
        private bool _showWorst = true;

        public bool ShowWorst
        {
            get => _showWorst;
            set

            {
                _showWorst = value;
                NotifyPropertyChanged();
            }
        }
        
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