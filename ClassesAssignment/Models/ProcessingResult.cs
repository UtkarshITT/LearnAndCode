using System.Collections.Generic;

namespace SolidDataProcessor.Models
{
    public class ProcessingResult
    {
        public int RecordsProcessed { get; set; }
        public int ErrorCount { get; set; }
        public IReadOnlyList<string> ErrorMessages => _errorMessages;
        public ProcessingStatistics Statistics { get; set; }
        public List<DataRecord> Records { get; set; } = new List<DataRecord>();

        internal readonly List<string> _errorMessages = new List<string>();
    }
}
