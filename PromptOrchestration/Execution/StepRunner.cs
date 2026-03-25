namespace PromptOrchestration;

public class StepRunner
{
	private readonly IStepRegistry stepRegistry;
	private readonly IExecutionConditionEvaluator executionConditionEvaluator;
	private readonly IWorkflowTracer workflowTracer;

	public StepRunner(
		IStepRegistry stepRegistry,
		IExecutionConditionEvaluator executionConditionEvaluator,
		IWorkflowTracer workflowTracer)
	{
		this.stepRegistry = stepRegistry;
		this.executionConditionEvaluator = executionConditionEvaluator;
		this.workflowTracer = workflowTracer;
	}

	public string runStep(
		WorkflowDefinition workflowDefinition,
		StepDefinition stepDefinition,
		string input,
		WorkflowRunContext workflowRunContext)
	{
		return runStepInternal(
			workflowDefinition,
			stepDefinition,
			input,
			workflowRunContext,
			new HashSet<string>(StringComparer.OrdinalIgnoreCase));
	}

	private string runStepInternal(
		WorkflowDefinition workflowDefinition,
		StepDefinition stepDefinition,
		string input,
		WorkflowRunContext workflowRunContext,
		HashSet<string> visitedStepIds)
	{
		if (stepDefinition.conditionDefinition is not null)
		{
			bool canRun = executionConditionEvaluator.shouldRun(
				stepDefinition.conditionDefinition,
				workflowRunContext);

			if (!canRun)
			{
				workflowTracer.stepSkipped(workflowRunContext, stepDefinition);
				return input;
			}
		}

		for (int attempt = 1; attempt <= stepDefinition.retryPolicy.maxAttempts; attempt++)
		{
			workflowTracer.stepStarted(workflowRunContext, stepDefinition, attempt);

			try
			{
				IWorkflowStep workflowStep = stepRegistry.create(stepDefinition.stepType);
				StepExecutionContext stepExecutionContext = new(input, stepDefinition, workflowRunContext);
				string output = workflowStep.execute(stepExecutionContext);
				workflowTracer.stepCompleted(workflowRunContext, stepDefinition, output);
				return output;
			}
			catch (Exception exception)
			{
				workflowTracer.stepFailed(workflowRunContext, stepDefinition, attempt, exception);

				bool hasNextAttempt = attempt < stepDefinition.retryPolicy.maxAttempts;

				if (hasNextAttempt && stepDefinition.retryPolicy.delayMilliseconds > 0)
				{
					Thread.Sleep(stepDefinition.retryPolicy.delayMilliseconds);
				}
			}
		}

		if (string.IsNullOrWhiteSpace(stepDefinition.fallbackStepId))
		{
			throw new WorkflowExecutionException(
				$"Step '{stepDefinition.stepId}' failed without fallback after " +
				$"{stepDefinition.retryPolicy.maxAttempts} attempts.");
		}

		if (!visitedStepIds.Add(stepDefinition.stepId))
		{
			throw new WorkflowExecutionException(
				$"Circular fallback detected for step '{stepDefinition.stepId}'.");
		}

		StepDefinition fallbackStepDefinition = workflowDefinition.getStep(stepDefinition.fallbackStepId);
		workflowTracer.stepFallbackTriggered(workflowRunContext, stepDefinition, fallbackStepDefinition);

		return runStepInternal(
			workflowDefinition,
			fallbackStepDefinition,
			input,
			workflowRunContext,
			visitedStepIds);
	}
}
