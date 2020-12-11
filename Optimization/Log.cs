using System;
using System.IO;

namespace Optimization
{
    public class Log
    {
        private string _path;
        private static Log _instance;
        private Log(string path)
        {
            _path = path;
            File.Delete(_path);
        }

        public static void Create(string path)
        {
            _instance = new Log(path);
        }

        public static Log GetInstance()
        {
            return _instance;
        }

        public static void AddToLog(string line)
        {
            File.AppendAllText(_instance._path,line + Environment.NewLine);
        }
    }
}