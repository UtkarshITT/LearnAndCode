namespace PromptOrchestration.Steps;

public class FailingStep : IWorkflowStep
{
	public string stepType => "FAILING";

	public string execute(StepExecutionContext stepExecutionContext)
	{
		throw new InvalidOperationException("Simulated transient AI provider failure.");
	}
}
