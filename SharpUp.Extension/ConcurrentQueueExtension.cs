using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Collections.Concurrent
{
    public static class ConcurrentQueueExtension
    {
        public static Task<T[]> DequeueAsync<T>(this ConcurrentQueue<T> queue, uint limit)
        {
            return Task.Run(() =>
            {
                List<T> items = new List<T>();
                while (limit > 0 && queue.Count > 0)
                {
                    if (!queue.TryDequeue(out T item)) throw new Exception("Unable to dequeue");
                    items.Add(item);
                }
                return items.ToArray();
            });
        }
    }
}