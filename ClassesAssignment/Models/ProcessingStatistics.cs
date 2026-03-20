namespace SolidDataProcessor.Models
{
    public class ProcessingStatistics
    {
        public int TotalRecords { get; set; }
        public int ErrorCount { get; set; }
        public double TotalValue { get; set; }
        public double AverageValue { get; set; }
    }
}
