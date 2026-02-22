using System.Collections.Generic;
using SolidDataProcessor.Interfaces;
using SolidDataProcessor.Models;

namespace SolidDataProcessor.Services
{
    public class CsvDataParser : IDataParser
    {
        private const int MinimumFieldCount = 3;
        private const int IdColumnIndex = 0;
        private const int NameColumnIndex = 1;
        private const int ValueColumnIndex = 2;
        private const int DateColumnIndex = 3;

        public IEnumerable<DataRecord> Parse(IEnumerable<string> lines, IList<string> errors)
        {
            var records = new List<DataRecord>();

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var fields = line.Split(',');

                if (fields.Length < MinimumFieldCount)
                {
                    errors.Add($"Invalid line format: {line}");
                    continue;
                }

                if (!double.TryParse(fields[ValueColumnIndex].Trim(), out double parsedValue))
                {
                    errors.Add($"Non-numeric value for record: {fields[IdColumnIndex].Trim()}");
                    continue;
                }

                records.Add(new DataRecord
                {
                    Id = fields[IdColumnIndex].Trim(),
                    Name = fields[NameColumnIndex].Trim(),
                    Value = parsedValue,
                    Date = fields.Length > DateColumnIndex ? fields[DateColumnIndex].Trim() : string.Empty
                });
            }

            return records;
        }
    }
}
