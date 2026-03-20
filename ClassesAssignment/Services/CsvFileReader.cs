using System.Collections.Generic;
using System.IO;
using SolidDataProcessor.Interfaces;

namespace SolidDataProcessor.Services
{
    public class CsvFileReader : IDataReader
    {
        public IEnumerable<string> ReadLines(string filePath)
        {
            return File.ReadAllLines(filePath);
        }
    }
}
