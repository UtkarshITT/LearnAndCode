using System.Collections.Generic;
using SolidDataProcessor.Interfaces;
using SolidDataProcessor.Models;

namespace SolidDataProcessor.Services
{
    public class RecordValidator : IDataValidator
    {
        public IEnumerable<DataRecord> Validate(
            IEnumerable<DataRecord> records,
            IList<string> errors,
            out int rejectedCount)
        {
            var validRecords = new List<DataRecord>();
            rejectedCount = 0;

            foreach (var record in records)
            {
                if (IsValid(record, errors))
                {
                    validRecords.Add(record);
                }
                else
                {
                    rejectedCount++;
                }
            }

            return validRecords;
        }

        private bool IsValid(DataRecord record, IList<string> errors)
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(record.Id))
            {
                errors.Add("Record is missing an ID");
                isValid = false;
            }

            if (string.IsNullOrEmpty(record.Name))
            {
                errors.Add($"Record {record.Id} is missing a name");
                isValid = false;
            }

            return isValid;
        }
    }
}
