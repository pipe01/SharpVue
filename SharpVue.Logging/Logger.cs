using System;

namespace SharpVue.Logging
{
    public static class Logger
    {
        public static LogLevel Level { get; set; } = LogLevel.Info;

        public static IReceiver Receiver { get; set; } = new ConsoleReceiver();

        public static void Log(LogLevel level, string str)
        {
            if (level >= Level)
                Receiver.Log(level, str);
        }

        public static void Log<T>(LogLevel level, string format, T arg)
        {
            if (level >= Level)
                Receiver.Log(level, string.Format(format, arg?.ToString()));
        }
        
        public static void Log<T1, T2>(LogLevel level, string format, T1 arg1, T2 arg2)
        {
            if (level >= Level)
                Receiver.Log(level, string.Format(format, arg1?.ToString(), arg2?.ToString()));
        }
        
        public static void Log<T1, T2, T3>(LogLevel level, string format, T1 arg1, T2 arg2, T3 arg3)
        {
            if (level >= Level)
                Receiver.Log(level, string.Format(format, arg1?.ToString(), arg2?.ToString(), arg3?.ToString()));
        }

        public static void Debug(string msg) => Log(LogLevel.Debug, msg);
        public static void Debug<T>(string msg, T arg) => Log(LogLevel.Debug, msg, arg);
        public static void Debug<T1, T2>(string msg, T1 arg1, T2 arg2) => Log(LogLevel.Debug, msg, arg1, arg2);
        public static void Debug<T1, T2, T3>(string msg, T1 arg1, T2 arg2, T3 arg3) => Log(LogLevel.Debug, msg, arg1, arg2, arg3);

        public static void Verbose(string msg) => Log(LogLevel.Verbose, msg);
        public static void Verbose<T>(string msg, T arg) => Log(LogLevel.Verbose, msg, arg);
        public static void Verbose<T1, T2>(string msg, T1 arg1, T2 arg2) => Log(LogLevel.Verbose, msg, arg1, arg2);
        public static void Verbose<T1, T2, T3>(string msg, T1 arg1, T2 arg2, T3 arg3) => Log(LogLevel.Verbose, msg, arg1, arg2, arg3);

        public static void Info(string msg) => Log(LogLevel.Info, msg);
        public static void Info<T>(string msg, T arg) => Log(LogLevel.Info, msg, arg);
        public static void Info<T1, T2>(string msg, T1 arg1, T2 arg2) => Log(LogLevel.Info, msg, arg1, arg2);
        public static void Info<T1, T2, T3>(string msg, T1 arg1, T2 arg2, T3 arg3) => Log(LogLevel.Info, msg, arg1, arg2, arg3);

        public static void Warn(string msg) => Log(LogLevel.Warning, msg);
        public static void Warn<T>(string msg, T arg) => Log(LogLevel.Warning, msg, arg);
        public static void Warn<T1, T2>(string msg, T1 arg1, T2 arg2) => Log(LogLevel.Warning, msg, arg1, arg2);
        public static void Warn<T1, T2, T3>(string msg, T1 arg1, T2 arg2, T3 arg3) => Log(LogLevel.Warning, msg, arg1, arg2, arg3);

        public static void Error(string msg) => Log(LogLevel.Error, msg);
        public static void Error<T>(string msg, T arg) => Log(LogLevel.Error, msg, arg);
        public static void Error<T1, T2>(string msg, T1 arg1, T2 arg2) => Log(LogLevel.Error, msg, arg1, arg2);
        public static void Error<T1, T2, T3>(string msg, T1 arg1, T2 arg2, T3 arg3) => Log(LogLevel.Error, msg, arg1, arg2, arg3);
    }
}
