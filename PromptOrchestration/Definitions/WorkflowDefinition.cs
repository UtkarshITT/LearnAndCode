namespace PromptOrchestration;

public class WorkflowDefinition
{
	private readonly IReadOnlyList<StepDefinition> steps;
	private readonly Dictionary<string, StepDefinition> stepsById;

	public string workflowName { get; }

	public IReadOnlyList<StepDefinition> workflowSteps => steps;

	public WorkflowDefinition(string workflowName, IEnumerable<StepDefinition> workflowSteps)
	{
		steps = workflowSteps.ToList();

		if (steps.Count == 0)
		{
			throw new ArgumentException("A workflow must contain at least one step.", nameof(workflowSteps));
		}

		this.workflowName = workflowName;
		stepsById = steps.ToDictionary(step => step.stepId, StringComparer.OrdinalIgnoreCase);
	}

	public StepDefinition getStep(string stepId)
	{
		if (!stepsById.TryGetValue(stepId, out StepDefinition? stepDefinition))
		{
			throw new WorkflowExecutionException(
				$"Workflow '{workflowName}' does not contain step '{stepId}'.");
		}

		return stepDefinition;
	}
}
