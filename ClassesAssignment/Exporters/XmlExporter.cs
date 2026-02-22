using System.Collections.Generic;
using System.IO;
using SolidDataProcessor.Interfaces;
using SolidDataProcessor.Models;

namespace SolidDataProcessor.Exporters
{
    public class XmlExporter : IDataExporter
    {
        public string SupportedFormat => "xml";

        public void Export(IEnumerable<DataRecord> records, string filePath)
        {
            var xmlLines = new List<string>
            {
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>",
                "<records>"
            };

            foreach (var record in records)
            {
                xmlLines.Add("  <record>");
                xmlLines.Add($"    <id>{record.Id}</id>");
                xmlLines.Add($"    <name>{record.Name}</name>");
                xmlLines.Add($"    <value>{record.Value}</value>");
                xmlLines.Add($"    <date>{record.Date}</date>");
                xmlLines.Add($"    <doubled_value>{record.DoubledValue}</doubled_value>");
                xmlLines.Add($"    <squared_value>{record.SquaredValue}</squared_value>");
                xmlLines.Add("  </record>");
            }

            xmlLines.Add("</records>");
            File.WriteAllLines(filePath, xmlLines);
        }
    }
}
