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
        /// <summary>
        /// Command Pattern for a specifc Parameter
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// MaxValue for ParameterRange
        /// </summary>
        public long MaxValue { get; set; } = long.MaxValue;

        /// <summary>
        /// MinValue for Parameter Range
        /// </summary>
        public long MinValue { get; set; } = long.MinValue;

        /// <summary>
        /// Length of array (pre defined for index)
        /// </summary>
        public int ArrayLength{ get; set; }

    }
}
