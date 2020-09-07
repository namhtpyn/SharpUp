using System;
using System.Collections.Generic;
using System.Threading;

namespace SharpUp.ScheduleJob
{
    public class ScheduleJob : IDisposable
    {
        private List<Timer> _data = new List<Timer>();

        public ScheduleJob()
        {
        }

        ~ScheduleJob()
        {
            Dispose();
        }

        public int Add(Action<object> action, DateTime startTime, TimeSpan interval)
        {
            var dueTime = TimeSpan.FromMilliseconds(Math.Max(0, (startTime - DateTime.Now).TotalMilliseconds));
            _data.Add(new Timer(new TimerCallback(action), null, dueTime, interval));
            return _data.Count;
        }

        public int Add(Action<object> action, DateTime startTime)
        {
            return Add(action, startTime, Timeout.InfiniteTimeSpan);
        }

        public void RemoveAt(int index)
        {
            _data[index].Change(Timeout.Infinite, Timeout.Infinite);
            _data.RemoveAt(index);
        }

        public void Clear()
        {
            _data.ForEach(a => a.Change(Timeout.Infinite, Timeout.Infinite));
            _data.Clear();
        }

        public void Dispose()
        {
            _data.ForEach(a => a.Change(Timeout.Infinite, Timeout.Infinite));
            _data = null;
        }
    }
}