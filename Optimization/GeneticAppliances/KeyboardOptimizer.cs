using Optimization.GeneticAlgorithms;
using Optimization.Parameters;

namespace Optimization.GeneticAppliances
{
    static class KeyboardOptimizer
    {
        public static void Optimize(OptimizationParameters optimizationParameters)
        {
            var keyboardOptimizer = new GeneticKeyboard(optimizationParameters);
            var result = keyboardOptimizer.Run();
            keyboardOptimizer.WriteResult(result);
        }
    }
}