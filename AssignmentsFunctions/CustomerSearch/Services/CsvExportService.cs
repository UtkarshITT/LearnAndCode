using System.Text;
using CustomerSearch.Interfaces;
using CustomerSearch.Models;

namespace CustomerSearch.Services;

public class CsvExportService : IExportService
{
    private const string Header = "CustomerId,CompanyName,ContactName,Country";

    /// <summary>
    /// Exports customer list to CSV format
    /// </summary>
    public string Export(List<Customer> customers)
    {
        var stringBuilder = new StringBuilder();
        
        // Add header row
        stringBuilder.AppendLine(Header);

        // Add data rows
        foreach (var customer in customers)
        {
            stringBuilder.AppendLine($"{customer.CustomerId},{customer.CompanyName},{customer.ContactName},{customer.Country}");
        }

        return stringBuilder.ToString();
    }
}
