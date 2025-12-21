using CustomerSearch.Models;

namespace CustomerSearch.Interfaces;

public interface IExportService
{
    string Export(List<Customer> customers);
}
