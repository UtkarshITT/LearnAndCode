using PromptOrchestration.Conditions;
using PromptOrchestration.Steps;

namespace PromptOrchestration;

public static class Program
{
	public static void Main()
	{
		IStepRegistry stepRegistry = createStepRegistry();
		IExecutionConditionEvaluator executionConditionEvaluator = new MetadataEqualsConditionEvaluator();
		IWorkflowTracer workflowTracer = new ConsoleWorkflowTracer();
		StepRunner stepRunner = new(stepRegistry, executionConditionEvaluator, workflowTracer);
		WorkflowEngine workflowEngine = new(stepRunner, workflowTracer);

		WorkflowDefinition workflowDefinition = createWorkflowDefinition();

		IReadOnlyDictionary<string, string> metadata = new Dictionary<string, string>
		{
			["targetLanguage"] = "Spanish"
		};

		string input = "A compact wireless keyboard with quiet keys and long battery life.";
		workflowEngine.run(workflowDefinition, input, metadata);
	}

	private static IStepRegistry createStepRegistry()
	{
		StepRegistry stepRegistry = new();
		stepRegistry.register(new GenerateStep());
		stepRegistry.register(new RefineToneStep());
		stepRegistry.register(new SummarizeStep());
		stepRegistry.register(new TranslateStep());
		stepRegistry.register(new FailingStep());
		stepRegistry.register(new FallbackStep());
		return stepRegistry;
	}

	private static WorkflowDefinition createWorkflowDefinition()
	{
		List<StepDefinition> stepDefinitions =
		[
			new StepDefinition(
				stepId: "generateDescription",
				stepType: "GENERATE",
				configuration: StepConfiguration.from(("prefix", "Generated product description:"))),
			new StepDefinition(
				stepId: "refineTone",
				stepType: "REFINE_TONE",
				configuration: StepConfiguration.from(("tone", "confident"))),
			new StepDefinition(stepId: "summarize", stepType: "SUMMARIZE"),
			new StepDefinition(
				stepId: "conditionalTranslate",
				stepType: "TRANSLATE",
				configuration: StepConfiguration.from(("targetLanguage", "Spanish")),
				conditionDefinition: new ConditionDefinition(
					conditionType: "MetadataEquals",
					metadataKey: "targetLanguage",
					expectedValue: "Spanish")),
			new StepDefinition(
				stepId: "unstableProviderCall",
				stepType: "FAILING",
				retryPolicy: new RetryPolicy(maxAttempts: 2, delayMilliseconds: 100),
				fallbackStepId: "safeFallback"),
			new StepDefinition(stepId: "safeFallback", stepType: "FALLBACK", runInWorkflow: false)
		];

		return new WorkflowDefinition("ProductDescriptionPipeline", stepDefinitions);
	}
}
