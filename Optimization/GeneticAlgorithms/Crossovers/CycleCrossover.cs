//
// namespace Optimization.GeneticAlgorithms.Crossovers
// {
//     internal class CycleCrossover : Crossover
//     {
//         public CycleCrossover(double[][] distancesMatrix) : base(distancesMatrix)
//         {
//
//         }
//
//         public override int[] GenerateOffspring(int[][] parents)
//         {
//             var length = parent1.Length;
//             int[] offspring = new int[length];
//             int[] filledCheck = new int[length];
//             for (int i = 0; i <= filledCheck.Length; i++)
//             {
//                 filledCheck[i] = 0;
//             }
//
//             int war;
//             int position = 0;
//             for (int i = 0; i <= parent1.Length; i++)
//             {
//                 if (filledCheck[position] != 0)
//                 {
//                     for (int j = 0; j <= parent1.Length; j++)
//                     {
//                         if (filledCheck[j] == 0)
//                         {
//                             offspring[j] = parent2[j];
//                         }
//                     }
//
//                     break;
//                 }
//                 else
//
//                     offspring[position] = parent1[position];
//
//                 war = parent2[position];
//                 filledCheck[position] = 1;
//                 for (int j = 0; j <= parent1.Length; j++)
//                 {
//                     if (parent1[j] == war)
//                     {
//                         position = parent1[j];
//                     }
//                 }
//             }
//
//             return offspring;
//         }
//     }
// }
