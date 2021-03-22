using Optimization.GeneticAlgorithms;
using Optimization.Parameters;

namespace Optimization.Keyboard
{
    static class KeyboardOptimizer
    {
        public static void Optimize(OptimizationParameters optimizationParameters)
        {
            var keyboardOptimizer = new GeneticKeyboard(optimizationParameters);
            keyboardOptimizer.Run();
        }
    }
}