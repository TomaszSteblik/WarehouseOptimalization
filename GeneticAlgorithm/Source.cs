using System;
using System.IO;

namespace GeneticAlgorithm
{
    public abstract class Source
    {
 

        public int[][] Data { get; protected set; }
        public int Size { get; protected set; }
        public string[] Cities { get; protected set; }
    }
    
    public class TxtFileSource : Source
    {
        public TxtFileSource(string pathToTxtFile)
        {
            string[] lines = File.ReadAllLines(pathToTxtFile);
            Size = lines.Length;
            Cities = new string[Size];
            Data = new int[Size][];
            for (int i = 0; i < Size; i++)
            {
                string[] numbersAndNames = lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                var numbersLength = numbersAndNames.Length;
                Data[i] = new int[Size];
                Cities[i] = numbersAndNames[0];
                for (int j = 0; j < numbersLength; j++)
                {
                    Data[i][j] = int.Parse(numbersAndNames[j]);
                }
            }
        }
    }
}