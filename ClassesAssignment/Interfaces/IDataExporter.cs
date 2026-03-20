using System.Collections.Generic;
using SolidDataProcessor.Models;

namespace SolidDataProcessor.Interfaces
{
    public interface IDataExporter
    {
        string SupportedFormat { get; }

        void Export(IEnumerable<DataRecord> records, string filePath);
    }
}
