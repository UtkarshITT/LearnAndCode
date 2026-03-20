using System.Collections.Generic;

namespace SolidDataProcessor.Interfaces
{
    public interface IDataReader
    {
        IEnumerable<string> ReadLines(string filePath);
    }
}
