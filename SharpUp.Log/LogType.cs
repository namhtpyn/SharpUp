using System;

namespace SharpUp.Log
{
    [Flags]
    public enum LogType
    {
        Info = 1,
        Debug = 2,
        Success = 4,
        Warning = 8,
        Error = 16
    }
}