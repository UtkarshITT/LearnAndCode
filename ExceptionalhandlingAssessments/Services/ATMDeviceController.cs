using ExceptionalhandlingAssessments.Exceptions;
using ExceptionalhandlingAssessments.Models;

namespace ExceptionalhandlingAssessments.Services;

public class ATMDeviceController
{
	private const string DEFAULT_DEVICE_ID = "DEV1";

	private readonly Dictionary<string, double> accountBalances;
	private readonly Dictionary<string, DeviceRecord> deviceRecords;

	public ATMDeviceController(DeviceRecord? defaultDeviceRecord = null)
	{
		accountBalances = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase)
		{
			["ACC1001"] = 700.00,
			["ACC1002"] = 150.00
		};

		deviceRecords = new Dictionary<string, DeviceRecord>(StringComparer.OrdinalIgnoreCase)
		{
			[DEFAULT_DEVICE_ID] = defaultDeviceRecord
				?? new DeviceRecord(DeviceStatus.Active, WifiConnectionStatus.Connected)
		};
	}

	public void withdraw(string accountId, double amount)
	{
		validateWithdrawalRequest(accountId, amount);
		var handle = getHandle(DEFAULT_DEVICE_ID);
		var record = retrieveActiveDeviceRecord(handle);
		ensureDeviceNetworkConnectivity(record);
		ensureSufficientFunds(accountId, amount);
		dispenseCash(handle, amount);
		debitAccount(accountId, amount);
	}

	private void validateWithdrawalRequest(string accountId, double amount)
	{
		if (string.IsNullOrWhiteSpace(accountId))
		{
			throw new ArgumentException("Account id is required.");
		}

		if (amount <= 0)
		{
			throw new InvalidWithdrawalAmountException("Withdrawal amount must be greater than zero.");
		}
	}

	private DeviceHandle getHandle(string deviceId)
	{
		if (deviceRecords.ContainsKey(deviceId))
		{
			return new DeviceHandle(deviceId);
		}

		return DeviceHandle.INVALID;
	}

	private DeviceRecord retrieveActiveDeviceRecord(DeviceHandle handle)
	{
		if (handle.isInvalid())
		{
			throw new DeviceUnavailableException("ATM device is not available.");
		}

		if (!deviceRecords.TryGetValue(handle.getValue(), out var record))
		{
			throw new DeviceUnavailableException("ATM device record was not found.");
		}

		if (record.isSuspended())
		{
			throw new DeviceLockedException("ATM device is locked and cannot serve requests.");
		}

		return record;
	}

	private void ensureDeviceNetworkConnectivity(DeviceRecord record)
	{
		if (!record.isWifiConnected())
		{
			throw new NetworkConnectionException("ATM device has no network connection.");
		}
	}

	private void ensureSufficientFunds(string accountId, double amount)
	{
		var availableBalance = getBalance(accountId);

		if (availableBalance < amount)
		{
			throw new InsufficientFundsException(
				$"Insufficient funds for account {accountId}. Available: {availableBalance:0.00}, requested: {amount:0.00}.");
		}
	}

	private double getBalance(string accountId)
	{
		if (!accountBalances.TryGetValue(accountId, out var balance))
		{
			throw new ArgumentException($"Account {accountId} was not found.");
		}

		return balance;
	}

	private void dispenseCash(DeviceHandle handle, double amount)
	{
		Console.WriteLine($"Dispensing {amount:0.00} from device {handle.getValue()}.");
	}

	private void debitAccount(string accountId, double amount)
	{
		accountBalances[accountId] = accountBalances[accountId] - amount;
	}
}
