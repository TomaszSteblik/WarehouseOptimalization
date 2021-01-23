using System;
using System.IO;
using System.Text;

namespace Optimization
{
    public class Log
    {
        private readonly string _path;
        private StringBuilder _stringBuilder;
        
        public Log(string path)
        {
            _stringBuilder = new StringBuilder();
            _path = path;
            File.Delete(_path);
        }
        
        public void SaveResult(int[] pointOrder, double length)
        {
            int size = pointOrder.Length;
            
            var result = new string[size + 1];
            result[0] = "Length = " + length;

            for (int i = 0; i < size; i++)
                result[i + 1] = pointOrder[i].ToString();

            File.WriteAllLines(_path, result);                
        }

        public void AddToLog(string message)
        {
            _stringBuilder.Append(message + Environment.NewLine);
        }

        public void Save()
        {
            File.WriteAllText(_path, _stringBuilder.ToString());
        }
    }
}