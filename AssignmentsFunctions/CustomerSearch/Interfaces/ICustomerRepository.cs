using CustomerSearch.Models;

namespace CustomerSearch.Interfaces;

public interface ICustomerRepository
{
    IQueryable<Customer> GetAll();
}
