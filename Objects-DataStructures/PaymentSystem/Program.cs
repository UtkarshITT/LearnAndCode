using ObjectsDataStructures.PaymentSystem.Domain;
using ObjectsDataStructures.PaymentSystem.Services;

var customer = new Customer("Avery", "Nguyen", new Wallet(25.00m));
var paperboy = new Paperboy();

var receipt = paperboy.CollectPayment(customer, 10.00m);

Console.WriteLine($"Payment successful: {receipt.WasPaid}");
Console.WriteLine($"Balance before: {receipt.BalanceBefore:C}");
Console.WriteLine($"Balance after: {receipt.BalanceAfter:C}");
