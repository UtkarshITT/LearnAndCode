namespace PromptOrchestration;

public class StepExecutionContext
{
	public string input { get; }

	public StepDefinition stepDefinition { get; }

	public WorkflowRunContext workflowRunContext { get; }

	public StepExecutionContext(string input, StepDefinition stepDefinition, WorkflowRunContext workflowRunContext)
	{
		this.input = input;
		this.stepDefinition = stepDefinition;
		this.workflowRunContext = workflowRunContext;
	}
}
