using CustomerSearch.Interfaces;
using CustomerSearch.Models;

namespace CustomerSearch.Services;

public class CustomerSearchService : ICustomerSearchService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerSearchService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    /// <summary>
    /// Search customers by country name
    /// </summary>
    public List<Customer> SearchByCountry(string country)
    {
        return _customerRepository.GetAll()
            .Where(c => c.Country.Contains(country, StringComparison.OrdinalIgnoreCase))
            .OrderBy(c => c.CustomerId)
            .ToList();
    }

    /// <summary>
    /// Search customers by company name
    /// </summary>
    public List<Customer> SearchByCompanyName(string companyName)
    {
        return _customerRepository.GetAll()
            .Where(c => c.CompanyName.Contains(companyName, StringComparison.OrdinalIgnoreCase))
            .OrderBy(c => c.CustomerId)
            .ToList();
    }

    /// <summary>
    /// Search customers by contact person name
    /// </summary>
    public List<Customer> SearchByContact(string contactName)
    {
        return _customerRepository.GetAll()
            .Where(c => c.ContactName.Contains(contactName, StringComparison.OrdinalIgnoreCase))
            .OrderBy(c => c.CustomerId)
            .ToList();
    }
}
