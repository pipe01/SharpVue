using System;

namespace SharpVue.Logging
{
    public class ConsoleReceiver : IReceiver
    {
        public void Log(LogLevel level, string message)
        {
            if (level != LogLevel.Info)
            {
                Console.Write(level switch
                {
                    LogLevel.Debug   => "[debg] ",
                    LogLevel.Verbose => "[verb] ",
                    LogLevel.Warning => "[warn] ",
                    LogLevel.Error   => "[erro] ",
                    _                => "[othe] "
                });
            }

            Console.WriteLine(message);
        }
    }
}
