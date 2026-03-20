using System.Collections.Generic;
using SolidDataProcessor.Models;

namespace SolidDataProcessor.Interfaces
{
    public interface IDataTransformer
    {
        IEnumerable<DataRecord> Transform(IEnumerable<DataRecord> records, string dateFormat);
    }
}
