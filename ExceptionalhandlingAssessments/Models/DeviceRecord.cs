namespace ExceptionalhandlingAssessments.Models;

public class DeviceRecord
{
	private readonly DeviceStatus status;
	private readonly WifiConnectionStatus wifiConnectionStatus;

	public DeviceRecord(DeviceStatus status, WifiConnectionStatus wifiConnectionStatus)
	{
		this.status = status;
		this.wifiConnectionStatus = wifiConnectionStatus;
	}

	public bool isSuspended()
	{
		return status == DeviceStatus.Suspended;
	}

	public bool isWifiConnected()
	{
		return wifiConnectionStatus == WifiConnectionStatus.Connected;
	}
}
