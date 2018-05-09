using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgumentParser
{

    /// <summary>
    /// PArameterAttribute as ParameterDefinition for Parser
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
   public  class ParameterInfoAttribute : Attribute
    {
        public string Command { get; set; }
        public long MaxValue { get; set; } = long.MaxValue;
        public long MinValue { get; set; } = long.MinValue;
        public int ArrayLenght { get; set; }

    }
}
