using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgumentParser
{
    #region TypeSwitch
    /// <summary>
    /// TypeSwitch class as Helper for type parsing
    /// </summary>
    public class TypeSwitch
    {

        private Dictionary<Type, Func<string, object>> matches = new Dictionary<Type, Func<string, object>>();
        public TypeSwitch Case<T>(Func<string, object> action) { matches.Add(typeof(T), (x) => { return action(x); }); return this; }
        public object Switch(Type x, string input) { return matches[x](input); }

        public bool ContainsType(Type o){ return matches.ContainsKey(o); }
    }
    #endregion
}
