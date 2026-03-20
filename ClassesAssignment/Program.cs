using System;
using System.Collections.Generic;
using SolidDataProcessor.Exporters;
using SolidDataProcessor.Interfaces;
using SolidDataProcessor.Logging;
using SolidDataProcessor.Models;
using SolidDataProcessor.Pipeline;
using SolidDataProcessor.Services;
using SolidDataProcessor.Utilities;

namespace SolidDataProcessor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new SampleDataGenerator().Generate("input.csv", 50);

            var configuration = new ProcessingConfiguration
            {
                ShouldValidate = true,
                ShouldTransform = true,
                DateFormat = "MM/dd/yyyy",
                BatchSize = 50
            };

            ILogger logger = new FileLogger();

            var pipeline = new DataProcessingPipeline(
                dataReader: new CsvFileReader(),
                dataParser: new CsvDataParser(),
                dataValidator: new RecordValidator(),
                dataTransformer: new RecordTransformer(),
                statisticsCalculator: new StatisticsCalculator(),
                logger: logger,
                outputExporter: new CsvExporter(),
                configuration: configuration);

            var result = pipeline.Execute("input.csv", "output.csv");

            DisplayStatistics(result);

            var exporterRegistry = new ExporterRegistry();
            exporterRegistry.Register(new CsvExporter());
            exporterRegistry.Register(new JsonExporter());
            exporterRegistry.Register(new XmlExporter());

            exporterRegistry.Export(result.Records, "output.json", "json");
            exporterRegistry.Export(result.Records, "output.xml", "xml");

            try
            {
                exporterRegistry.Export(result.Records, "output_test.json", "json");
                exporterRegistry.Export(result.Records, "output_test.xml", "xml");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Export error: {ex.Message}");
            }

            var filteredRecords = FilterByMinimumValue(result.Records, minimumValue: 100);
            Console.WriteLine($"\nFiltered records (value >= 100): {filteredRecords.Count}");

            Console.WriteLine($"\nRecords processed: {result.RecordsProcessed}");
            Console.WriteLine($"Errors:            {result.ErrorCount}");
        }

        private static void DisplayStatistics(ProcessingResult result)
        {
            Console.WriteLine("\n=== Processing Statistics ===");
            Console.WriteLine($"Total Records:  {result.Statistics.TotalRecords}");
            Console.WriteLine($"Error Count:    {result.Statistics.ErrorCount}");
            Console.WriteLine($"Total Value:    {result.Statistics.TotalValue:F2}");
            Console.WriteLine($"Average Value:  {result.Statistics.AverageValue:F2}");

            if (result.ErrorMessages.Count > 0)
            {
                Console.WriteLine("\n=== Errors ===");
                foreach (var error in result.ErrorMessages)
                {
                    Console.WriteLine($"  - {error}");
                }
            }
        }

        private static List<DataRecord> FilterByMinimumValue(
            List<DataRecord> records,
            double minimumValue)
        {
            return records.FindAll(record => record.Value >= minimumValue);
        }
    }
}
