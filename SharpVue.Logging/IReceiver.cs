namespace SharpVue.Logging
{
    public interface IReceiver
    {
        void Log(LogLevel level, string message);
    }
}
