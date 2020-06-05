using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SharpUp.Log
{
    public class Log : IDisposable
    {
        public event EventHandler<string> OnWrite;

        public event EventHandler<string> OnError;

        private BlockingCollection<string> _data = new BlockingCollection<string>();
        private LogType _types = LogType.Info | LogType.Success | LogType.Warning | LogType.Error;
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
            _ = Task.Run(Write);
        }

        ~Log()
        {
            Dispose();
        }

        public void Info(params object[] args)
        {
            if (_types.HasFlag(LogType.Info)) Append(LogType.Info, args);
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
            _data.TryAdd(string.Format("{0:dd-MM-yyyy HH:mm:ss.fff}:[{1}] - {2}", DateTime.Now, type.ToString(), string.Join(",", args)));
        }

        private void Write()
        {
            try
            {
                while (true)
                {
                    var msg = _data.Take(_isLogging.Token);
                    try
                    {
                        if (!Directory.Exists(_dir)) Directory.CreateDirectory(_dir);
                        File.AppendAllText(Path.Combine(_dir, string.Format("{0:yyyy-MM-dd}.txt", DateTime.Now)), msg + Environment.NewLine);
                        OnWrite?.Invoke(this, msg);
                    }
                    catch (Exception ex)
                    {
                        OnError?.Invoke(this, ex.Message);
                    }
                }
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