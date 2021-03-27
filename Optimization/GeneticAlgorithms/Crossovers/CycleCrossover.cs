//
// namespace Optimization.GeneticAlgorithms.Crossovers
// {
//     internal class CycleCrossover : Crossover
//     {
//         public CycleCrossover()
//         {
//
//         }
//         protected int[] GenerateOffsprings(int[] parent1, int[] parent2)
//         {
//             var length = parent1.Length;
//             int[] offspring = new int[length];
//             int[] filledCheck = new int[length];
//             for (int i = 0; i <= filledCheck.Length; i++)
//             {
//                 filledCheck[i] = 0;
//             }
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
//                     break;
//                 }
//                 else
//
//                     offspring[position] = parent1[position];
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
//
//         public override int[][] GenerateOffsprings(int[][] parents, int numParentsForOneChild = 2)
//         {
//             var parentsLength = parents.Length;
//             var amountOfChildren = parentsLength / 2;
//             int[][] offsprings = new int[amountOfChildren][];
//
//             for (int c = 0, j = 0; c < amountOfChildren; j += 2, c++)
//             {
//                 offsprings[c] = GenerateOffsprings(parents[j], parents[j + 1]);
//             }
//             return offsprings;
//         }
//     }
// }
