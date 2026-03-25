namespace PromptOrchestration;

public class ConsoleWorkflowTracer : IWorkflowTracer
{
	public void workflowStarted(WorkflowRunContext workflowRunContext, string input)
	{
		Console.WriteLine($"[{workflowRunContext.runId}] Workflow started: {workflowRunContext.workflowName}");
		Console.WriteLine($"Input: {input}");
	}

	public void stepStarted(WorkflowRunContext workflowRunContext, StepDefinition stepDefinition, int attemptNumber)
	{
		Console.WriteLine(
			$"[{workflowRunContext.runId}] Step started: {stepDefinition.stepId} " +
			$"({stepDefinition.stepType}), attempt {attemptNumber}");
	}

	public void stepSkipped(WorkflowRunContext workflowRunContext, StepDefinition stepDefinition)
	{
		Console.WriteLine($"[{workflowRunContext.runId}] Step skipped: {stepDefinition.stepId}");
	}

	public void stepCompleted(WorkflowRunContext workflowRunContext, StepDefinition stepDefinition, string output)
	{
		Console.WriteLine($"[{workflowRunContext.runId}] Step completed: {stepDefinition.stepId}");
		Console.WriteLine($"Output: {output}");
	}

	public void stepFailed(
		WorkflowRunContext workflowRunContext,
		StepDefinition stepDefinition,
		int attemptNumber,
		Exception exception)
	{
		Console.WriteLine(
			$"[{workflowRunContext.runId}] Step failed: {stepDefinition.stepId}, " +
			$"attempt {attemptNumber}, reason: {exception.Message}");
	}

	public void stepFallbackTriggered(
		WorkflowRunContext workflowRunContext,
		StepDefinition stepDefinition,
		StepDefinition fallbackStepDefinition)
	{
		Console.WriteLine(
			$"[{workflowRunContext.runId}] Step fallback triggered: {stepDefinition.stepId} -> " +
			$"{fallbackStepDefinition.stepId}");
	}

	public void workflowCompleted(WorkflowRunContext workflowRunContext, string output)
	{
		Console.WriteLine($"[{workflowRunContext.runId}] Workflow completed.");
		Console.WriteLine($"Final output: {output}");
	}

	public void workflowFailed(WorkflowRunContext workflowRunContext, Exception exception)
	{
		Console.WriteLine(
			$"[{workflowRunContext.runId}] Workflow failed: {exception.Message}");
	}
}
