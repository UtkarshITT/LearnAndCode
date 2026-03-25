namespace PromptOrchestration;

public class StepConfiguration
{
	private readonly Dictionary<string, string> valuesByKey;

	public StepConfiguration()
	{
		valuesByKey = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
	}

	public StepConfiguration(IEnumerable<KeyValuePair<string, string>> values)
	{
		valuesByKey = new Dictionary<string, string>(values, StringComparer.OrdinalIgnoreCase);
	}

	public string getValueOrDefault(string key, string defaultValue)
	{
		if (valuesByKey.TryGetValue(key, out string? value))
		{
			return value;
		}

		return defaultValue;
	}

	public static StepConfiguration from(params (string key, string value)[] values)
	{
		IEnumerable<KeyValuePair<string, string>> pairs = values
			.Select(value => new KeyValuePair<string, string>(value.key, value.value));

		return new StepConfiguration(pairs);
	}
}
