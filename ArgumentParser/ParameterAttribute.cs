using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgumentParser
{
    [AttributeUsage(AttributeTargets.Parameter)]
    class ParameterAttribute : Attribute
    {
        public string Command { get; set; }
        public long MaxValue { get; set; }
        public long MinValue { get; set; }
        public int ArrayLenght { get; set; }

    }
}
