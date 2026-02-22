using System.Collections.Generic;
using System.IO;
using SolidDataProcessor.Interfaces;
using SolidDataProcessor.Models;

namespace SolidDataProcessor.Exporters
{    public class JsonExporter : IDataExporter
    {
        public string SupportedFormat => "json";

        public void Export(IEnumerable<DataRecord> records, string filePath)
        {
            var recordList = new List<DataRecord>(records);
            var jsonLines = new List<string> { "[" };

            for (int index = 0; index < recordList.Count; index++)
            {
                var record = recordList[index];
                bool isLastRecord = index == recordList.Count - 1;
                string trailingComma = isLastRecord ? string.Empty : ",";

                jsonLines.Add(
                    $"  {{" +
                    $"\"id\": \"{record.Id}\", " +
                    $"\"name\": \"{record.Name}\", " +
                    $"\"value\": {record.Value}, " +
                    $"\"date\": \"{record.Date}\", " +
                    $"\"doubled_value\": {record.DoubledValue}, " +
                    $"\"squared_value\": {record.SquaredValue}" +
                    $"}}{trailingComma}");
            }

            jsonLines.Add("]");
            File.WriteAllLines(filePath, jsonLines);
        }
    }
}
