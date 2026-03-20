using System.Collections.Generic;
using SolidDataProcessor.Models;

namespace SolidDataProcessor.Interfaces
{
    public interface IDataValidator
    {
        IEnumerable<DataRecord> Validate(
            IEnumerable<DataRecord> records,
            IList<string> errors,
            out int rejectedCount);
    }
}
