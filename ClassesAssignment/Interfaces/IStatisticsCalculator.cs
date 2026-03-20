using System.Collections.Generic;
using SolidDataProcessor.Models;

namespace SolidDataProcessor.Interfaces
{
    public interface IStatisticsCalculator
    {
        ProcessingStatistics Calculate(IEnumerable<DataRecord> records, int errorCount);
    }
}
