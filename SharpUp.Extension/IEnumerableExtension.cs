using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpUp.Extension
{
    public static class IEnumerableExtension
    {
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T, int> action)
        {
            var count = enumeration.Count();
            for (var i = 0; i < count; i++) action(enumeration.ElementAt(i), i);
        }

        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            var count = enumeration.Count();
            for (var i = 0; i < count; i++) action(enumeration.ElementAt(i));
        }
    }
}