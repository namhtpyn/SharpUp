using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharpUp.ScheduleJob
{
    public class ScheduleJob
    {
        private List<JobTimer> _data = new List<JobTimer>();
        public int Count => _data.Count;

        public ScheduleJob()
        {
        }

        ~ScheduleJob()
        {
            this.Clear();
            _data = null;
        }

        public int Add(Action action, DateTime startTime, TimeSpan interval, bool overlap = true)
        {
            _data.Add(new JobTimer(action, startTime, interval, overlap));
            return _data.Count;
        }

        public int Add(Action action, DateTime startTime)
        {
            return Add(action, startTime, Timeout.InfiniteTimeSpan, false);
        }

        public int Add(Func<Task> action, DateTime startTime, TimeSpan interval, bool overlap = true)
        {
            _data.Add(new JobTimer(action, startTime, interval, overlap));
            return _data.Count;
        }

        public int Add(Func<Task> action, DateTime startTime)
        {
            return Add(action, startTime, Timeout.InfiniteTimeSpan, false);
        }

        public JobTimer Get(int index)
        {
            return _data[index];
        }

        public void RemoveAt(int index)
        {
            _data[index].Dispose();
            _data.RemoveAt(index);
        }

        public void Clear()
        {
            _data.ForEach(a => a.Dispose());
            _data.Clear();
        }
    }
}