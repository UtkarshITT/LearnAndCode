using CustomerSearch.Interfaces;
using CustomerSearch.Models;

namespace CustomerSearch.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly List<Customer> _customers;

    public CustomerRepository()
    {
        // Sample data for demonstration
        _customers = new List<Customer>
        {
            new("ALFKI", "Alfreds Futterkiste", "Maria Anders", "Germany"),
            new("ANATR", "Ana Trujillo Emparedados", "Ana Trujillo", "Mexico"),
            new("ANTON", "Antonio Moreno Taquería", "Antonio Moreno", "Mexico"),
            new("BERGS", "Berglunds snabbköp", "Christina Berglund", "Sweden"),
            new("BLAUS", "Blauer See Delikatessen", "Hanna Moos", "Germany"),
            new("BOLID", "Bólido Comidas preparadas", "Martín Sommer", "Spain"),
            new("BONAP", "Bon app'", "Laurence Lebihan", "France"),
            new("BOTTM", "Bottom-Dollar Markets", "Elizabeth Lincoln", "Canada")
        };
    }

    public IQueryable<Customer> GetAll()
    {
        return _customers.AsQueryable();
    }
}
