using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ArgumentParser
{

    /// <summary>
    /// Class for parsing TerminalInput Arguments and call defined Method in a Object with it's parameters
    /// </summary>
    public class ArgumentParser
    {

        #region varDef
        private object argumentObject;
        #endregion

        #region constructor
        /// <summary>
        /// Instantiates a new ArgumentParser Object with it's ArgumentObject parameter
        /// </summary>
        /// <param name="argumentObject">Object instance which contains public methods with <see cref="CommandAttribute"/> for Regex Command definition</param>
        public ArgumentParser(object argumentObject)
        {
            this.argumentObject = argumentObject;
        }
        #endregion

        #region Parse
        /// <summary>
        /// Parses an generic String command and executes the defined Method behind
        /// </summary>
        /// <typeparam name="T">Type of Returnobject</typeparam>
        /// <param name="command">Command input for parse</param>
        /// <returns></returns>
        public T Parse<T>(string command)
        {
            MethodInfo meth = GetMatchingMethod(command);

            if (meth == null)
            {
                return default(T);
            }


            CommandAttribute commandAttribute = meth.GetCustomAttribute<CommandAttribute>();

            Regex r1 = new Regex(commandAttribute.Command);

            Match match = r1.Match(command);
            if (!match.Success)
            {
                return default(T);
            }

            var groups = match.Groups;
            var param = meth.GetParameters();

            if (groups.Count - 1 != param.Length && !param.Any(p=>p.ParameterType.IsArray))
            {
                return default(T);
            }

            int idx = 0;
            int groupOffset = 0;
            object[] parameters = new object[param.Length];
            foreach (var p in param)
            {

                if (p.ParameterType.IsArray)
                {
                    parameters[idx] = ParseArray(groups, p,idx,ref groupOffset);
                }
                else
                {
                    parameters[idx] = ParseValue(groups[idx+ groupOffset + 1].Value, p.ParameterType);

                    ValidateParameterRestrictions(parameters[idx], p);
                }

                idx++;

            }


            return (T)meth.Invoke(this.argumentObject, parameters);        
        }
        #endregion

        #region ValidateParameterRestrictions
        private static void ValidateParameterRestrictions(object input, ParameterInfo p)
        {
            ParameterInfoAttribute parameterAttribute = p.GetCustomAttribute<ParameterInfoAttribute>();
            if (parameterAttribute != null && input.IsNumericType())
            {
                if (Convert.ToDecimal(input) > parameterAttribute.MaxValue)
                {
                    throw new ArgumentParserException(string.Format("Given parameter '{0}' is greater than defined MaxValue {1}", input, parameterAttribute.MaxValue));
                }
                else if (Convert.ToDecimal(input) < parameterAttribute.MinValue)
                {
                    throw new ArgumentParserException(string.Format("Given parameter '{0}' is smaller than defined MinValue {1}", input, parameterAttribute.MinValue));
                }
            }
        }
        #endregion


        #region ParseArray
        private object ParseArray(GroupCollection value, ParameterInfo p,int idx,ref int groupOffset)
        {
            ParameterInfoAttribute parameterAttribute = p.GetCustomAttribute<ParameterInfoAttribute>();
            ArrayList list = new ArrayList();            
            for (int i = 0; i < parameterAttribute.ArrayLenght; i++)
            {
                object parsedValue = ParseValue(value[idx + i + 1].Value, p.ParameterType.GetElementType());
                ValidateParameterRestrictions(parsedValue, p);
                list.Add(parsedValue);
            }
            groupOffset += parameterAttribute.ArrayLenght-1;            
            return (object)list.ToArray(p.ParameterType.GetElementType());            
        }
        #endregion

        #region GetMatchingMethod
        private MethodInfo GetMatchingMethod(string command)
        {
            var i = argumentObject.GetType().GetMethods();

            foreach (MethodInfo item in i)
            {
                var attributes = item.GetCustomAttributes<CommandAttribute>().ToList();

                if (attributes.Count == 0)
                {
                    continue;
                }

                CommandAttribute commandAtt = attributes[0];


                Regex r1 = new Regex(commandAtt.Command);

                Match match = r1.Match(command);
                if (match.Success)
                {
                    return item;
                }

            }
            return null;
        }
        #endregion

        #region ParseValue
        private object ParseValue(string input, Type destinationType)
        {
            object parsedValue = null;
            var ts = new TypeSwitch()
                  .Case<string>((string x) => { return x; })
                  .Case<bool>((string x) => { return bool.Parse(x); })
                  .Case<int>((string x) => { return int.Parse(x); })
                  .Case<long>((string x) => { return long.Parse(x); })
                  .Case<float>((string x) => { return float.Parse(x); })
                  .Case<double>((string x) => { return double.Parse(x); })
                  .Case<decimal>((string x) => { return decimal.Parse(x); });

            parsedValue = ts.Switch(destinationType, input);

            return parsedValue;
        }
        #endregion
    }

}
