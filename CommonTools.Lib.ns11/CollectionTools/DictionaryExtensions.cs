using CommonTools.Lib.ns11.ExceptionTools;
using System;
using System.Collections.Generic;

namespace CommonTools.Lib.ns11.CollectionTools
{
    public static class DictionaryExtensions
    {
        public static void IfFound<TKey, TVal>(
            this Dictionary<TKey, TVal> dict, 
            TKey key, 
            Action<TVal> action,
            bool errorIfMissing = false)
        {
            if (dict.TryGetValue(key, out TVal value))
                action(value);
            else if (errorIfMissing)
                throw Fault.BadData($"Dictionary ‹{typeof(TVal).Name}› does not contain key [{key}].");
        }
    }
}
