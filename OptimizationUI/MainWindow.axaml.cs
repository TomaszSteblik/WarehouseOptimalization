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
            Optimization.Parameters.OptimizationParameters optimizationParameters = new OptimizationParameters
            {
                OptimizationMethod = (OptimizationMethod) this.FindControl<ComboBox>("OptimizationBox").SelectedIndex,
                CrossoverMethod = (Crossover.CrossoverType) this.FindControl<ComboBox>("CrossoverBox").SelectedIndex,
                MutationMethod = (Mutation.MutationType) this.FindControl<ComboBox>("MutationBox").SelectedIndex,
                EliminationMethod =
                    (Elimination.EliminationType) this.FindControl<ComboBox>("EliminationBox").SelectedIndex,
                SelectionMethod = (Selection.SelectionType) this.FindControl<ComboBox>("SelectionBox").SelectedIndex,
                Use2opt = (bool) this.FindControl<CheckBox>("2OptBox")!.IsChecked!,
                MutationProbability = Int32.Parse(this.FindControl<TextBox>("MutationTextBox").Text),
                PopulationSize = Int32.Parse(this.FindControl<TextBox>("PopulationTextBox").Text),
                ChildrenPerGeneration = Int32.Parse(this.FindControl<TextBox>("ChildrenTextBox").Text),
                TerminationValue = Int32.Parse(this.FindControl<TextBox>("TerminationTextBox").Text),
                DataPath = this.FindControl<TextBox>("DataTextBox").Text
                
            };


            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = Optimization.OptimizationWork.FindShortestPath(optimizationParameters);
            stopwatch.Stop();
            this.FindControl<TextBlock>("ResultText").Text = result.ToString();
            this.FindControl<TextBlock>("ElapsedText").Text = stopwatch.Elapsed.ToString();

        }
    }
}