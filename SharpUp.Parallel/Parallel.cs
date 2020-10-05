using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpUp.Parallel
{
    public static class Parallel
    {
        public static Task ForEachAsync<T>(IEnumerable<T> list, Action<T> action)
        {
            return Task.WhenAll(list.Select(e => Task.Run(() => action(e))).ToArray());
        }

        public static Task ForEachAsync<T>(IEnumerable<T> list, Func<T, Task> action)
        {
            return Task.WhenAll(list.Select(e => action(e)).ToArray());
        }
    }
}