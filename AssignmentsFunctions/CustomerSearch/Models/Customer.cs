namespace CustomerSearch.Models;
public class Customer
{
    public string CustomerId { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public Customer() { }

    public Customer(string customerId, string companyName, string contactName, string country)
    {
        CustomerId = customerId;
        CompanyName = companyName;
        ContactName = contactName;
        Country = country;
    }
}
