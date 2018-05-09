using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgumentParser
{
    /// <summary>
    /// Custom Exception for CommandParser Errors
    /// </summary>
 public class ArgumentParserException : Exception
    {
        #region ctor
        public ArgumentParserException(string message) : base(message)
        {

        }

        public ArgumentParserException(string message,Exception cause): base(message,cause)
        {
          
        }
        #endregion
    }
}
