using System;

namespace SharpUp.Log
{
    [Flags]
    public enum LogType
    {
        Info = 0,
        Success = 1,
        Warning = 2,
        Error = 4
    }
}