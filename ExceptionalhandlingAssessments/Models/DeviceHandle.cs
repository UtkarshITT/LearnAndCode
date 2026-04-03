namespace ExceptionalhandlingAssessments.Models;

public readonly struct DeviceHandle
{
	public static readonly DeviceHandle INVALID = new(string.Empty);

	private readonly string value;

	public DeviceHandle(string value)
	{
		this.value = value;
	}

	public bool isInvalid()
	{
		return string.IsNullOrWhiteSpace(value);
	}

	public string getValue()
	{
		return value;
	}
}
