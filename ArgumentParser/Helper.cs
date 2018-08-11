using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgumentParser
{
    #region Helper
    /// <summary>
    /// Helper class for severl helper methods
    /// </summary>
    public static class Helper
    {

        /// <summary>
        /// Indicates whether an object type is numeric  
        /// </summary>
        /// <param name="o">ObjectReference to check type (Extention)</param>
        /// <returns>true if given objct is numeric</returns>
        public static bool IsNumericType(this object o)
        {
            switch (Type.GetTypeCode(o.GetType())) //Switch without break(!) for all numeric types
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }
    }
    #endregion
}
