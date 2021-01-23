using System;
using System.IO;
using System.Text;

namespace Optimization
{
    public class Log
    {
        private readonly string _path;
        private readonly string _resultPath;
        private readonly StringBuilder _stringBuilder;
        private readonly bool _logEnabled;
        
        public Log(OptimizationParameters optimizationParameters)
        {
            _stringBuilder = new StringBuilder();
            _path = optimizationParameters.LogPath;
            _resultPath = optimizationParameters.ResultPath;
            _logEnabled = optimizationParameters.LogEnabled;
            File.Delete(_path);
        }
        
        public void SaveResult(int[] pointOrder, double length)
        {
            int size = pointOrder.Length;
            
            var result = new string[size + 1];
            result[0] = "Length = " + length;

            for (int i = 0; i < size; i++)
                result[i + 1] = pointOrder[i].ToString();

            File.WriteAllLines(_resultPath, result);                
        }

        public void AddToLog(string message)
        {
            if(_logEnabled) _stringBuilder.Append(message + Environment.NewLine);
        }

        public void SaveLog()
        {
            if(_logEnabled) File.WriteAllText(_path, _stringBuilder.ToString());
        }
    }
}