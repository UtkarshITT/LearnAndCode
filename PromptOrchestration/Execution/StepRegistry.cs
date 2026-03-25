namespace PromptOrchestration;

public class StepRegistry : IStepRegistry
{
	private readonly Dictionary<string, IWorkflowStep> stepsByType;

	public StepRegistry()
	{
		stepsByType = new Dictionary<string, IWorkflowStep>(StringComparer.OrdinalIgnoreCase);
	}

	public void register(IWorkflowStep workflowStep)
	{
		ArgumentNullException.ThrowIfNull(workflowStep);

		if (stepsByType.ContainsKey(workflowStep.stepType))
		{
			throw new InvalidOperationException(
				$"A step with type '{workflowStep.stepType}' is already registered.");
		}

		stepsByType[workflowStep.stepType] = workflowStep;
	}

	public IWorkflowStep create(string stepType)
	{
		if (!stepsByType.TryGetValue(stepType, out IWorkflowStep? workflowStep))
		{
			throw new WorkflowExecutionException($"No step registered for type '{stepType}'.");
		}

		return workflowStep;
	}
}
