namespace PromptOrchestration;

public class StepDefinition
{
	public string stepId { get; }

	public string stepType { get; }

	public StepConfiguration configuration { get; }

	public RetryPolicy retryPolicy { get; }

	public string? fallbackStepId { get; }

	public bool runInWorkflow { get; }

	public ConditionDefinition? conditionDefinition { get; }

	public StepDefinition(
		string stepId,
		string stepType,
		StepConfiguration? configuration = null,
		RetryPolicy? retryPolicy = null,
		string? fallbackStepId = null,
		bool runInWorkflow = true,
		ConditionDefinition? conditionDefinition = null)
	{
		this.stepId = stepId;
		this.stepType = stepType;
		this.configuration = configuration ?? new StepConfiguration();
		this.retryPolicy = retryPolicy ?? RetryPolicy.noRetry();
		this.fallbackStepId = fallbackStepId;
		this.runInWorkflow = runInWorkflow;
		this.conditionDefinition = conditionDefinition;
	}
}
