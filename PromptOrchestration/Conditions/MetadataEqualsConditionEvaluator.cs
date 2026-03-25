namespace PromptOrchestration.Conditions;

public class MetadataEqualsConditionEvaluator : IExecutionConditionEvaluator
{
	private const string metadataEqualsType = "MetadataEquals";

	public bool shouldRun(ConditionDefinition conditionDefinition, WorkflowRunContext workflowRunContext)
	{
		if (!conditionDefinition.conditionType.Equals(metadataEqualsType, StringComparison.OrdinalIgnoreCase))
		{
			throw new WorkflowExecutionException(
				$"Unsupported condition type '{conditionDefinition.conditionType}'.");
		}

		string actualValue = workflowRunContext.getMetadataOrDefault(conditionDefinition.metadataKey);

		return actualValue.Equals(conditionDefinition.expectedValue, StringComparison.OrdinalIgnoreCase);
	}
}
