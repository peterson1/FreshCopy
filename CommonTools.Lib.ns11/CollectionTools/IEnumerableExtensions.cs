using CommonTools.Lib.ns11.ExceptionTools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonTools.Lib.ns11.CollectionTools
{
    public static class IEnumerableExtensions
    {
        public static T GetOne<T>(this IEnumerable<T> collection, Func<T, bool> predicate, string predicateDescription)
        {
            if (collection == null)
                throw Fault.BadData("collection == NULL");

            if (!collection.Any())
                throw Fault.BadData("Collection has no items");

            var matches = collection.Where(predicate);

            if (matches.Count() == 1)
                return matches.First();

            var typ = typeof(T).Name;

            if (matches.Count() == 0)
                throw Fault.BadData($"No ‹{typ}› found where [{predicate}].");
            else
                throw Fault.BadData($"Multiple ‹{typ}› found where [{predicate}].");
        }
    }
}
