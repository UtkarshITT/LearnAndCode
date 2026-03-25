namespace PromptOrchestration.Steps;

public class GenerateStep : IWorkflowStep
{
	public string stepType => "GENERATE";

	public string execute(StepExecutionContext stepExecutionContext)
	{
		string prefix = stepExecutionContext.stepDefinition.configuration
			.getValueOrDefault("prefix", "Generated:");

		return $"{prefix} {stepExecutionContext.input}";
	}
}
