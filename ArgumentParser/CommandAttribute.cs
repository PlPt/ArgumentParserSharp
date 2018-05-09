using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgumentParser
{
   
    /// <summary>
    /// CommandAttribute class as AttributeDefinition of commands
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        /// <summary>
        /// Regex Command to match String command and it's parameters
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// Description to display for a help
        /// </summary>
        public string Description { get; set; }

    }
}
