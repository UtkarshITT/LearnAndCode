namespace SolidDataProcessor.Interfaces
{
    public interface ILogger
    {
        void Log(string message);
        void Flush(string logFilePath);
    }
}
