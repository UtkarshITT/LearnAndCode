using System.Collections.Generic;
using System.Linq;
using SolidDataProcessor.Interfaces;
using SolidDataProcessor.Models;

namespace SolidDataProcessor.Services
{
    public class StatisticsCalculator : IStatisticsCalculator
    {
        public ProcessingStatistics Calculate(IEnumerable<DataRecord> records, int errorCount)
        {
            var recordList = records.ToList();
            double totalValue = recordList.Sum(record => record.Value);

            return new ProcessingStatistics
            {
                TotalRecords = recordList.Count,
                ErrorCount = errorCount,
                TotalValue = totalValue,
                AverageValue = recordList.Count > 0 ? totalValue / recordList.Count : 0
            };
        }
    }
}
