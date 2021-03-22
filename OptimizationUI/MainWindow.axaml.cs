using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Optimization.GeneticAlgorithms.Crossovers;
using Optimization.GeneticAlgorithms.Eliminations;
using Optimization.GeneticAlgorithms.Mutations;
using Optimization.GeneticAlgorithms.Selections;
using Optimization.Parameters;

namespace OptimizationUI
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void Button_OnClick(object? sender, RoutedEventArgs e)
        {
            Optimization.Parameters.OptimizationParameters optimizationParameters = new OptimizationParameters();
            optimizationParameters.OptimizationMethod =
                (OptimizationMethod) this.FindControl<ComboBox>("OptimizationBox").SelectedIndex;
            optimizationParameters.CrossoverMethod = (Crossover.CrossoverType) this.FindControl<ComboBox>("CrossoverBox").SelectedIndex;
            optimizationParameters.MutationMethod = (Mutation.MutationType) this.FindControl<ComboBox>("MutationBox").SelectedIndex;
            optimizationParameters.EliminationMethod = (Elimination.EliminationType) this.FindControl<ComboBox>("EliminationBox").SelectedIndex;
            optimizationParameters.SelectionMethod = (Selection.SelectionType) this.FindControl<ComboBox>("SelectionBox").SelectedIndex;
            optimizationParameters.Use2opt = (bool) this.FindControl<CheckBox>("2optBox").IsChecked;
            optimizationParameters.MutationProbability = Int32.Parse(this.FindControl<TextBox>("MutationTextBox").Text);
            optimizationParameters.PopulationSize = Int32.Parse(this.FindControl<TextBox>("PopulationTextBox").Text);
            optimizationParameters.ChildrenPerGeneration = Int32.Parse(this.FindControl<TextBox>("ChildrenTextBox").Text);
            optimizationParameters.TerminationValue = Int32.Parse(this.FindControl<TextBox>("TerminationTextBox").Text);
            optimizationParameters.DataPath = this.FindControl<TextBox>("DataTextBox").Text;


            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = Optimization.OptimizationWork.FindShortestPath(optimizationParameters);
            stopwatch.Stop();
            this.FindControl<TextBlock>("ResultText").Text = result.ToString();
            this.FindControl<TextBlock>("ElapsedText").Text = stopwatch.Elapsed.ToString();

        }
    }
}