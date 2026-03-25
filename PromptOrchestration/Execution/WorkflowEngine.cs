namespace PromptOrchestration;

public class WorkflowEngine
{
	private readonly StepRunner stepRunner;
	private readonly IWorkflowTracer workflowTracer;

	public WorkflowEngine(StepRunner stepRunner, IWorkflowTracer workflowTracer)
	{
		this.stepRunner = stepRunner;
		this.workflowTracer = workflowTracer;
	}

	public string run(
		WorkflowDefinition workflowDefinition,
		string input,
		IReadOnlyDictionary<string, string>? metadata = null)
	{
		WorkflowRunContext workflowRunContext = new(workflowDefinition.workflowName, metadata);
		workflowTracer.workflowStarted(workflowRunContext, input);

		string output = input;

		try
		{
			foreach (StepDefinition stepDefinition in workflowDefinition.workflowSteps)
			{
				if (!stepDefinition.runInWorkflow)
				{
					continue;
				}

				output = stepRunner.runStep(workflowDefinition, stepDefinition, output, workflowRunContext);
			}

			workflowTracer.workflowCompleted(workflowRunContext, output);
			return output;
		}
		catch (Exception exception)
		{
			workflowTracer.workflowFailed(workflowRunContext, exception);
			throw;
		}
	}
}
