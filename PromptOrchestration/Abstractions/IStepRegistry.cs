namespace PromptOrchestration;

public interface IStepRegistry
{
	void register(IWorkflowStep workflowStep);

	IWorkflowStep create(string stepType);
}
