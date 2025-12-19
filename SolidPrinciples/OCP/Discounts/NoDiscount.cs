namespace OCP.Discounts;
public class NoDiscount : IDiscount
{
    public string Name => "No Discount";

    public decimal Calculate(decimal price) => price;
}
