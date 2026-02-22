using System;
using System.Collections.Generic;
using SolidDataProcessor.Interfaces;
using SolidDataProcessor.Models;

namespace SolidDataProcessor.Services
{
    public class RecordTransformer : IDataTransformer
    {
        public IEnumerable<DataRecord> Transform(IEnumerable<DataRecord> records, string dateFormat)
        {
            var transformedRecords = new List<DataRecord>();

            foreach (var record in records)
            {
                transformedRecords.Add(ApplyTransformations(record, dateFormat));
            }

            return transformedRecords;
        }

        private DataRecord ApplyTransformations(DataRecord record, string dateFormat)
        {
            return new DataRecord
            {
                Id = record.Id,
                Name = record.Name.ToUpper(),
                Value = record.Value,
                Date = ParseAndFormatDate(record.Date, dateFormat),
                DoubledValue = record.Value * 2,
                SquaredValue = record.Value * record.Value
            };
        }

        private string ParseAndFormatDate(string rawDate, string dateFormat)
        {
            if (DateTime.TryParse(rawDate, out DateTime parsedDate))
            {
                return parsedDate.ToString(dateFormat);
            }

            return rawDate;
        }
    }
}
