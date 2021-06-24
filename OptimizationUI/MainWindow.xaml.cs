using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAlgorithms.Crossovers.ConflictResolvers;
using Optimization.GeneticAppliances.TSP;
using Optimization.GeneticAppliances.Warehouse;
using Optimization.Parameters;
using OptimizationUI.ViewModels;

namespace OptimizationUI
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            DataContext = new MainViewModel();
            
        }
        
    }
}
