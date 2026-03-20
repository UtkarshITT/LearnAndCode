using System;
using System.Collections.Generic;
using System.IO;
using SolidDataProcessor.Interfaces;
using SolidDataProcessor.Models;

namespace SolidDataProcessor.Pipeline
{
    public class DataProcessingPipeline
    {
        private readonly IDataReader _dataReader;
        private readonly IDataParser _dataParser;
        private readonly IDataValidator _dataValidator;
        private readonly IDataTransformer _dataTransformer;
        private readonly IStatisticsCalculator _statisticsCalculator;
        private readonly ILogger _logger;
        private readonly IDataExporter _outputExporter;
        private readonly ProcessingConfiguration _configuration;

        private const string DefaultLogFilePath = "processing.log";

        public DataProcessingPipeline(
            IDataReader dataReader,
            IDataParser dataParser,
            IDataValidator dataValidator,
            IDataTransformer dataTransformer,
            IStatisticsCalculator statisticsCalculator,
            ILogger logger,
            IDataExporter outputExporter,
            ProcessingConfiguration configuration)
        {
            _dataReader = dataReader ?? throw new ArgumentNullException(nameof(dataReader));
            _dataParser = dataParser ?? throw new ArgumentNullException(nameof(dataParser));
            _dataValidator = dataValidator ?? throw new ArgumentNullException(nameof(dataValidator));
            _dataTransformer = dataTransformer ?? throw new ArgumentNullException(nameof(dataTransformer));
            _statisticsCalculator = statisticsCalculator ?? throw new ArgumentNullException(nameof(statisticsCalculator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _outputExporter = outputExporter ?? throw new ArgumentNullException(nameof(outputExporter));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public ProcessingResult Execute(string inputFilePath, string outputFilePath)
        {
            var result = new ProcessingResult();

            try
            {
                var rawLines = ReadData(inputFilePath);
                int errorCountBefore = result._errorMessages.Count;
                var parsedRecords = ParseData(rawLines, result._errorMessages);
                result.ErrorCount += result._errorMessages.Count - errorCountBefore;

                if (_configuration.ShouldValidate)
                {
                    parsedRecords = ValidateData(parsedRecords, result._errorMessages, out int rejectedCount);
                    result.ErrorCount += rejectedCount;
                }

                if (_configuration.ShouldTransform)
                {
                    parsedRecords = TransformData(parsedRecords);
                }

                result.Records = new List<DataRecord>(parsedRecords);
                result.Statistics = _statisticsCalculator.Calculate(result.Records, result.ErrorCount);
                result.RecordsProcessed = result.Records.Count;

                WriteOutput(result.Records, outputFilePath);
                _logger.Flush(DefaultLogFilePath);

                PrintSummary(result);
            }
            catch (Exception ex)
            {
                result.ErrorCount++;
                result._errorMessages.Add($"Fatal error: {ex.Message}");
                _logger.Log($"FATAL ERROR: {ex.Message}");
                Console.WriteLine($"Processing failed: {ex.Message}");
            }

            return result;
        }

        // ── private stage methods ────────────────────────────────────────────

        private IEnumerable<string> ReadData(string inputFilePath)
        {
            EnsureFileExists(inputFilePath);
            _logger.Log($"Reading input file: {inputFilePath}");
            var lines = _dataReader.ReadLines(inputFilePath);
            _logger.Log("File read successfully");
            return lines;
        }

        private IEnumerable<DataRecord> ParseData(IEnumerable<string> lines, IList<string> errors)
        {
            _logger.Log("Parsing data...");
            var records = _dataParser.Parse(lines, errors);
            _logger.Log("Parsing complete");
            return records;
        }

        private IEnumerable<DataRecord> ValidateData(
            IEnumerable<DataRecord> records,
            IList<string> errors,
            out int rejectedCount)
        {
            _logger.Log("Validating data...");
            var validRecords = _dataValidator.Validate(records, errors, out rejectedCount);
            _logger.Log($"Validation complete — {rejectedCount} records rejected");
            return validRecords;
        }

        private IEnumerable<DataRecord> TransformData(IEnumerable<DataRecord> records)
        {
            _logger.Log("Transforming data...");
            var transformedRecords = _dataTransformer.Transform(records, _configuration.DateFormat);
            _logger.Log("Transformation complete");
            return transformedRecords;
        }

        private void WriteOutput(IEnumerable<DataRecord> records, string outputFilePath)
        {
            _logger.Log($"Writing output to: {outputFilePath}");
            _outputExporter.Export(records, outputFilePath);
            _logger.Log("Output written");
        }

        private static void EnsureFileExists(string filePath)
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
        }

        private static void PrintSummary(ProcessingResult result)
        {
            Console.WriteLine("Processing complete!");
            Console.WriteLine($"Records processed: {result.RecordsProcessed}");
            Console.WriteLine($"Errors:            {result.ErrorCount}");
        }
    }
}
