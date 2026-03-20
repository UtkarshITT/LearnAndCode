using System.Collections.Generic;
using SolidDataProcessor.Models;

namespace SolidDataProcessor.Interfaces
{
    public interface IDataParser
    {
        IEnumerable<DataRecord> Parse(IEnumerable<string> lines, IList<string> errors);
    }
}
