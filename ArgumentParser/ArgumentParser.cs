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
        /// args params for paremeters which can not pared by regex, will be passed to the method call
        /// </summary>
        /// <typeparam name="T">Type of Returnobject</typeparam>
        /// <param name="command">Command input for parse</param>
        /// <param name="args">Optional Arguments for Runtime Method, can be declaed typed in the destination method</param>
        /// <returns></returns>
        public T Parse<T>(string command, params object[] args)
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

            if (groups.Count - 1 + args.Length != param.Length && !param.Any(p => p.ParameterType.IsArray)) //Check if number of params matches
            {
                return default(T);
            }

            int idx = 0;
            int groupOffset = 0;
            object[] parameters = new object[param.Length];
            foreach (var p in param)
            {
                int currentOffset = idx + groupOffset + 1;
                if (groups.Count > currentOffset) //If param is from regex
                {

                    if (p.ParameterType.IsArray)
                    {
                        parameters[idx] = ParseArray(groups, p, idx, ref groupOffset);
                    }
                    else
                    {
                        parameters[idx] = ParseValue(groups[currentOffset].Value, p.ParameterType);

                        ValidateParameterRestrictions(parameters[idx], p);
                    }
                }
                else if (args.Length > currentOffset - groups.Count)
                {
                    if (args[currentOffset - groups.Count] is string)
                    {
                        parameters[idx] = ParseValue(args[currentOffset - groups.Count].ToString(), p.ParameterType);
                    }
                    else
                    {
                        parameters[idx] = args[currentOffset - groups.Count];
                    }

                }
                else
                {
                    throw new ArgumentParserException("The auto choosen method expects non regex parameters which are not given in curren context");
                }

                idx++;

            }


            return (T)meth.Invoke(this.argumentObject, parameters);
        }
        #endregion

        #region ValidateParameterRestrictions
        /// <summary>
        /// Validates an input object if it fullfills the definition of a parameterInfo
        /// </summary>
        /// <param name="input">Object to validate</param>
        /// <param name="p">ParameterInfo definition</param>
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
        /// <summary>
        /// Parses given Values from RegEx GroupCollection as typed array
        /// </summary>
        /// <param name="value">GroupCollectionValues</param>
        /// <param name="p">ParameterInfoAttibute of Array</param>
        /// <param name="idx">Index of GroupItem</param>
        /// <param name="groupOffset">GroupOffset dto increase</param>
        /// <returns>typed array boxed in an object</returns>
        private object ParseArray(GroupCollection value, ParameterInfo p, int idx, ref int groupOffset)
        {
            ParameterInfoAttribute parameterAttribute = p.GetCustomAttribute<ParameterInfoAttribute>();
            ArrayList list = new ArrayList();
            for (int i = 0; i < parameterAttribute.ArrayLength; i++)
            {
                object parsedValue = ParseValue(value[idx + i + 1].Value, p.ParameterType.GetElementType());
                ValidateParameterRestrictions(parsedValue, p);
                list.Add(parsedValue);
            }
            groupOffset += parameterAttribute.ArrayLength - 1;
            return (object)list.ToArray(p.ParameterType.GetElementType());
        }
        #endregion

        #region GetMatchingMethod
        /// <summary>
        /// Returns the Method with correspoding CommandAttribute definition
        /// </summary>
        /// <param name="command">Command to check definitions for</param>
        /// <returns>MethodInfo of Method to invoke</returns>
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
        /// <summary>
        /// Parses given String input to given destination Type
        /// </summary>
        /// <param name="input">String representation of Type</param>
        /// <param name="destinationType">Typte to wich the string should parsed</param>
        /// <returns>strongliy typed value boxed in an object</returns>
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
