using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpUp.ScheduleJob
{
    public class JobTimer : IDisposable
    {
        public DateTime StartTime { get; private set; }
        public TimeSpan Interval { get; private set; }

        public JobStatus Status { get; private set; }
        public bool Overlap { get; set; }
        private Timer _timer;
        private Action _action;
        private Func<Task> _func;

        public JobTimer(Action action, DateTime startTime, TimeSpan interval, bool overlap = true)
        {
            StartTime = startTime;
            Interval = interval;
            Overlap = overlap;
            _action = action;
            var dueTime = this.GetDueTime(startTime, interval);
            _timer = new Timer(new TimerCallback((o) => JobAction(action)), null, dueTime, interval);
            Status = JobStatus.Ready;
        }

        public JobTimer(Func<Task> action, DateTime startTime, TimeSpan interval, bool overlap = true)
        {
            StartTime = startTime;
            Interval = interval;
            Overlap = overlap;
            _func = action;
            var dueTime = this.GetDueTime(startTime, interval);
            _timer = new Timer(new TimerCallback((o) => JobAction(action)), null, dueTime, interval);
            Status = JobStatus.Ready;
        }

        ~JobTimer()
        {
            this.Dispose();
        }

        private void JobAction(Action action)
        {
            if (!Overlap && Status == JobStatus.Running) return;
            Status = JobStatus.Running;
            action();
            Status = JobStatus.Ready;
        }

        private async void JobAction(Func<Task> action)
        {
            if (!Overlap && Status == JobStatus.Running) return;
            Status = JobStatus.Running;
            await action();
            Status = JobStatus.Ready;
        }

        public TimeSpan GetDueTime(DateTime startTime, TimeSpan interval)
        {
            if (startTime < DateTime.Now && interval != Timeout.InfiniteTimeSpan)
                startTime = startTime.AddTicks(interval.Ticks * (long)Math.Ceiling((double)DateTime.Now.Subtract(startTime).Ticks / interval.Ticks));
            return TimeSpan.FromTicks(Math.Max(0, (startTime - DateTime.Now).Ticks));
        }

        public void Change(DateTime startTime, TimeSpan interval)
        {
            StartTime = startTime;
            Interval = interval;
            var dueTime = this.GetDueTime(startTime, interval);
            Change(dueTime, interval);
        }

        public void Change(TimeSpan dueTime, TimeSpan interval)
        {
            _timer.Change(dueTime, interval);
        }

        public void Run()
        {
            if (_action == null)
                Task.Run(() => JobAction(_func));
            else
                Task.Run(() => JobAction(_action));
        }

        public void Disable()
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);
            Status = JobStatus.Disabled;
        }

        public void Enable()
        {
            var dueTime = this.GetDueTime(StartTime, Interval);
            _timer.Change(dueTime, Interval);
            Status = JobStatus.Ready;
        }

        public void Dispose()
        {
            if (_timer == null) return;
            _timer.Dispose();
            _timer = null;
        }
    }
}