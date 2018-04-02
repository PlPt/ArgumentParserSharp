using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ArgumentParser
{
    public class ArgumentParser
    {

        #region varDef
        private object argumentObject;
        #endregion

        #region constructor
        public ArgumentParser(object argumentObject)
        {
            this.argumentObject = argumentObject;
        }
        #endregion

        #region Parse
        public T Parse<T>(string command)
        {
            MethodInfo meth = GetMatchingMethod(command);

            if(meth == null)
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

            if(groups.Count - 1 != param.Length)
            {
                return default(T);
            }

            int idx = 1;
            object[] parameters = new object[param.Length];
            foreach(var p in param)
            {
                parameters[idx - 1] = ParseValue(groups[idx].Value, p.ParameterType);
                idx++;

            }


          return   (T)meth.Invoke(this.argumentObject, parameters);

            //return "";
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
        private object ParseValue(string input,Type destinationType)
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

            parsedValue =  ts.Switch(destinationType,input);

            return parsedValue;
        }

        #endregion
    }

    public class TypeSwitch
    {
        
        Dictionary<Type, Func<string,object>> matches = new Dictionary<Type, Func<string,object>>();
        public TypeSwitch Case<T>(Func<string,object> action) { matches.Add(typeof(T), (x) => { return action(x); }); return this; }
        public object Switch(Type x,string input) { return  matches[x](input); }
    }
}
