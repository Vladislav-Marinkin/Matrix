namespace Worker
{
    public interface ILogger
    {
        void Log(string message, LogLevel level);
    }

    public enum LogLevel
    {
        Info,
        Warning,
        Error
    }

    public class ConsoleLogger : ILogger
    {
        public void Log(string message, LogLevel level)
        {
            Console.WriteLine($"[{level}] {message}");
        }
    }
}
