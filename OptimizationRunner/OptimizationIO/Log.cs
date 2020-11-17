using System;
using System.IO;

namespace OptimizationIO
{
    public class Log
    {
        private string _path;
        public Log(string path)
        {
            _path = path;
            File.Delete(_path);
        }

        public void AddToLog(string line)
        {
            File.AppendAllText(_path,line + Environment.NewLine);
        }
    }
}