using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SharpUp.Log
{
    public class Log : IDisposable
    {
        public event EventHandler<LogEntry> OnWrite;

        public event EventHandler<string> OnError;

        private BlockingCollection<LogEntry> _data = new BlockingCollection<LogEntry>();
        private LogType _types = LogType.Info | LogType.Debug | LogType.Success | LogType.Warning | LogType.Error;
        private CancellationTokenSource _isLogging = new CancellationTokenSource();
        private string _dir = "./logs";

        public Log(string dir, LogType types) : this(dir)
        {
            _types = types;
        }

        public Log(string dir) : this()
        {
            _dir = dir ?? throw new ArgumentNullException(nameof(dir));
        }

        public Log(LogType types) : this()
        {
            _types = types;
        }

        public Log()
        {
            _ = Task.Run(WriteFile);
        }

        ~Log()
        {
            Dispose();
        }

        public void SetType(LogType types)
        {
            _types = types;
        }

        public void Info(params object[] args)
        {
            if (_types.HasFlag(LogType.Info)) Append(LogType.Info, args);
        }

        public void Debug(params object[] args)
        {
            if (_types.HasFlag(LogType.Debug)) Append(LogType.Debug, args);
        }

        public void Success(params object[] args)
        {
            if (_types.HasFlag(LogType.Success)) Append(LogType.Success, args);
        }

        public void Warning(params object[] args)
        {
            if (_types.HasFlag(LogType.Warning)) Append(LogType.Warning, args);
        }

        public void Error(params object[] args)
        {
            if (_types.HasFlag(LogType.Error)) Append(LogType.Error, args);
        }

        private void Append(LogType type, object[] args)
        {
            _data.TryAdd(new LogEntry(type, string.Join(",", args)));
        }

        private void WriteFile()
        {
            try
            {
                while (true)
                {
                    var logEntry = _data.Take(_isLogging.Token);
                    try
                    {
                        if (!Directory.Exists(_dir)) Directory.CreateDirectory(_dir);
                        File.AppendAllText(Path.Combine(_dir, string.Format("{0:yyyy-MM-dd}.txt", DateTime.Now)), logEntry.ToString() + Environment.NewLine);
                        OnWrite?.Invoke(this, logEntry);
                    }
                    catch (Exception ex)
                    {
                        OnError?.Invoke(this, ex.Message);
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, ex.Message);
            }
        }

        public void Dispose()
        {
            _isLogging.Cancel();
        }
    }
}