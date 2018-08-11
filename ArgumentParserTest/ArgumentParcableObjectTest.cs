using ArgumentParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgumentParserTest
{
    class ArgumentParcableObjectTest 
    {
        #region varDef
        int x = 0;
        DateTime date;
        #endregion

        /// <summary>
        /// Ctor for Parsing
        /// </summary>
        public ArgumentParcableObjectTest(string inputData)
        {
          x =   int.Parse(inputData.Split(",".ToCharArray())[0]);
            date = DateTime.Parse(inputData.Split(",".ToCharArray())[1]);
        }

    }
}
