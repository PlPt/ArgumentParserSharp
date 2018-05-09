using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgumentParser
{
    class ArgumentParserException : Exception
    {
        public ArgumentParserException(string message) : base(message)
        {

        }

        public ArgumentParserException(string message,Exception cause): base(message,cause)
        {
          
        }
    }
}
