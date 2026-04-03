using ExceptionalhandlingAssessments.Exceptions;
using ExceptionalhandlingAssessments.Models;
using ExceptionalhandlingAssessments.Services;

var healthyController = new ATMDeviceController();
attemptWithdrawal("Happy Path", healthyController, "ACC1001", 200.00);

var lockedController = new ATMDeviceController(
	new DeviceRecord(DeviceStatus.Suspended, WifiConnectionStatus.Connected));
attemptWithdrawal("Device Locked", lockedController, "ACC1001", 100.00);

var disconnectedController = new ATMDeviceController(
	new DeviceRecord(DeviceStatus.Active, WifiConnectionStatus.Disconnected));
attemptWithdrawal("Connection Error", disconnectedController, "ACC1001", 100.00);

var lowBalanceController = new ATMDeviceController();
attemptWithdrawal("Insufficient Funds", lowBalanceController, "ACC1002", 500.00);

static void attemptWithdrawal(string scenarioName, ATMDeviceController controller, string accountId, double amount)
{
	Console.WriteLine($"Scenario: {scenarioName}");

	try
	{
		controller.withdraw(accountId, amount);
		Console.WriteLine($"Withdrawal completed for account {accountId}. Amount: {amount:0.00}");
	}
	catch (DeviceLockedException ex)
	{
		Console.WriteLine($"Withdrawal failed: {ex.Message}");
	}
	catch (NetworkConnectionException ex)
	{
		Console.WriteLine($"Withdrawal failed: {ex.Message}");
	}
	catch (InsufficientFundsException ex)
	{
		Console.WriteLine($"Withdrawal failed: {ex.Message}");
	}
	catch (Exception ex)
	{
		Console.WriteLine($"Withdrawal failed: {ex.Message}");
	}

	Console.WriteLine();
}
