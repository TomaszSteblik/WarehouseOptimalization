using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Optimization;
using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAlgorithms.Eliminations;
using Optimization.GeneticAlgorithms.Mutations;
using Optimization.GeneticAlgorithms.Selections;
using Optimization.Parameters;

namespace OptimizationUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DistanceViewModel _distanceViewModel;
        public MainWindow()
        {
            InitializeComponent();
            _distanceViewModel = new DistanceViewModel();
            DistancePanel.DataContext = _distanceViewModel;
            DistanceMethodComboBox.ItemsSource = Enum.GetValues(typeof(OptimizationMethod)).Cast<OptimizationMethod>();
            DistanceSelectionComboBox.ItemsSource = Enum.GetValues(typeof(SelectionMethod)).Cast<SelectionMethod>();
            DistanceCrossoverComboBox.ItemsSource = Enum.GetValues(typeof(CrossoverMethod)).Cast<CrossoverMethod>();
            DistanceEliminationComboBox.ItemsSource = Enum.GetValues(typeof(EliminationMethod)).Cast<EliminationMethod>();
            DistanceMutationComboBox.ItemsSource = Enum.GetValues(typeof(MutationMethod)).Cast<MutationMethod>();
        }

        private void DistanceStartButtonClick(object sender, RoutedEventArgs e)
        {
            OptimizationParameters parameters = _distanceViewModel as OptimizationParameters;
            List<double> results = new List<double>();
            for (int i = 0; i < Double.Parse(DistanceInstancesTextBox.Text); i++)
            {
                results.Add(OptimizationWork.FindShortestPath(parameters));
            }

            DistanceResultLabel.Content = $"Avg: {results.Average()}  Max: {results.Max()}  Min: {results.Min()}";
        }
    }
}