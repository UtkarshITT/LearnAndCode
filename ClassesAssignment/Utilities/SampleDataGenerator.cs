using System;
using System.Collections.Generic;
using System.IO;

namespace SolidDataProcessor.Utilities
{
    public class SampleDataGenerator
    {
        private readonly Random _random = new Random();

        public void Generate(string filePath, int recordCount)
        {
            var lines = new List<string>();

            for (int recordNumber = 1; recordNumber <= recordCount; recordNumber++)
            {
                string id = $"ID{recordNumber:D4}";
                string name = $"Item{recordNumber}";
                double value = _random.Next(10, 1000);
                DateTime date = DateTime.Now.AddDays(-_random.Next(0, 365));

                lines.Add($"{id},{name},{value},{date:yyyy-MM-dd}");
            }

            File.WriteAllLines(filePath, lines);
            Console.WriteLine($"Generated {recordCount} sample records in {filePath}");
        }
    }
}
