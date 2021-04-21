using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Optimization.GeneticAlgorithms.Modules
{
    public interface IModule
    {
        public string GetDesiredObject();

        public void RunAction(object obj);
    }

    public class GeneticModule<T> : IModule
    {
        public Action<T> Action { get; set; }

        public GeneticModule(Action<T> action)
        {
            Action = action;
        }

        public GeneticModule()
        {
            
        }

        public virtual string GetDesiredObject()
        {
            return "";
        }

        public virtual void RunAction(object obj)
        {

            Action((T) obj);
        }
    }

}