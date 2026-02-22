using System;
using System.IO;
using System.Text;
using SolidDataProcessor.Interfaces;

namespace SolidDataProcessor.Logging
{
    public class FileLogger : ILogger
    {
        private readonly StringBuilder _logBuffer = new StringBuilder();

        public void Log(string message)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _logBuffer.AppendLine($"[{timestamp}] {message}");
        }

        public void Flush(string logFilePath)
        {
            File.WriteAllText(logFilePath, _logBuffer.ToString());
        }
    }
}
