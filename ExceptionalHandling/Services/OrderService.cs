using ExceptionalHandling.Exceptions;

namespace ExceptionalHandling.Services;

public class OrderService
{
    public void ProcessOrder(int amount)
    {
        try
        {
            if (amount <= 0)
            {
                throw new InvalidOrderAmountException("Order amount must be greater than zero.");
            }

            Console.WriteLine("Order processed successfully.");
        }
        catch (InvalidOrderAmountException ex)
        {
            Console.WriteLine($"OrderService caught exception: {ex.Message}");
            throw;
        }
        finally
        {
            Console.WriteLine("OrderService finally block executed.");
        }
    }
}
