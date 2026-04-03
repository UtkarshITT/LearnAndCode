using ExceptionalHandling.Exceptions;
using ExceptionalHandling.Services;

var orderService = new OrderService();

try
{
    Console.WriteLine("Trying to process order...");
    orderService.ProcessOrder(-5);
}
catch (InvalidOrderAmountException ex)
{
    Console.WriteLine($"Caught custom exception: {ex.Message}");
}
finally
{
    Console.WriteLine("Finally block: cleanup code runs every time.");
}
