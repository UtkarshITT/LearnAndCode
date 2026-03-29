namespace OCP.Discounts;
public interface IDiscount
{
    string Name { get; }
    decimal Calculate(decimal price);
}
