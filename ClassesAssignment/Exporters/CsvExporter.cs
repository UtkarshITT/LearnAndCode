using System.Collections.Generic;
using System.IO;
using System.Linq;
using SolidDataProcessor.Interfaces;
using SolidDataProcessor.Models;

namespace SolidDataProcessor.Exporters
{
    public class CsvExporter : IDataExporter
    {
        public string SupportedFormat => "csv";

        public void Export(IEnumerable<DataRecord> records, string filePath)
        {
            var lines = new List<string> { "ID,NAME,VALUE,DATE,DOUBLED_VALUE,SQUARED_VALUE" };

            lines.AddRange(records.Select(record =>
                $"{record.Id},{record.Name},{record.Value}," +
                $"{record.Date},{record.DoubledValue},{record.SquaredValue}"));

            File.WriteAllLines(filePath, lines);
        }
    }
}
