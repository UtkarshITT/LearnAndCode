using System;
using System.Collections.Generic;
using SolidDataProcessor.Interfaces;
using SolidDataProcessor.Models;

namespace SolidDataProcessor.Exporters
{
    public class ExporterRegistry
    {
        private readonly Dictionary<string, IDataExporter> _exportersByFormat
            = new Dictionary<string, IDataExporter>();

        public void Register(IDataExporter exporter)
        {
            _exportersByFormat[exporter.SupportedFormat.ToLower()] = exporter;
        }

        public void Export(IEnumerable<DataRecord> records, string filePath, string format)
        {
            string normalizedFormat = format.ToLower();

            if (!_exportersByFormat.TryGetValue(normalizedFormat, out IDataExporter exporter))
            {
                throw new ArgumentException($"Unsupported format: {format}");
            }

            exporter.Export(records, filePath);
        }
    }
}
