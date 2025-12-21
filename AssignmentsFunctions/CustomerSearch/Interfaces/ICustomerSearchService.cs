using CustomerSearch.Models;

namespace CustomerSearch.Interfaces;

public interface ICustomerSearchService
{
    List<Customer> SearchByCountry(string country);
    List<Customer> SearchByCompanyName(string companyName);
    List<Customer> SearchByContact(string contactName);
}
