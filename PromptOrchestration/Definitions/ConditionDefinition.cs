namespace PromptOrchestration;

public class ConditionDefinition
{
	public string conditionType { get; }

	public string metadataKey { get; }

	public string expectedValue { get; }

	public ConditionDefinition(string conditionType, string metadataKey, string expectedValue)
	{
		this.conditionType = conditionType;
		this.metadataKey = metadataKey;
		this.expectedValue = expectedValue;
	}
}
