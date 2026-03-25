namespace PromptOrchestration;

public interface IWorkflowTracer
{
	void workflowStarted(WorkflowRunContext workflowRunContext, string input);

	void stepStarted(WorkflowRunContext workflowRunContext, StepDefinition stepDefinition, int attemptNumber);

	void stepSkipped(WorkflowRunContext workflowRunContext, StepDefinition stepDefinition);

	void stepCompleted(WorkflowRunContext workflowRunContext, StepDefinition stepDefinition, string output);

	void stepFailed(
		WorkflowRunContext workflowRunContext,
		StepDefinition stepDefinition,
		int attemptNumber,
		Exception exception);

	void stepFallbackTriggered(
		WorkflowRunContext workflowRunContext,
		StepDefinition stepDefinition,
		StepDefinition fallbackStepDefinition);

	void workflowCompleted(WorkflowRunContext workflowRunContext, string output);

	void workflowFailed(WorkflowRunContext workflowRunContext, Exception exception);
}
