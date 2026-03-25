namespace PromptOrchestration;

public interface IWorkflowStep
{
	string stepType { get; }

	string execute(StepExecutionContext stepExecutionContext);
}
