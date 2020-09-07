using System;

namespace SharpUp.Log
{
    [Flags]
    public enum LogType
    {
        Info = 0,
        Debug = 1,
        Success = 2,
        Warning = 4,
        Error = 8
    }
}