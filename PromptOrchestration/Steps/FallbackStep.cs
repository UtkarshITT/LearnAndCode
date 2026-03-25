namespace PromptOrchestration.Steps;

public class FallbackStep : IWorkflowStep
{
	public string stepType => "FALLBACK";

	public string execute(StepExecutionContext stepExecutionContext)
	{
		return $"[Fallback output] {stepExecutionContext.input}";
	}
}
