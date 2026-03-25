namespace PromptOrchestration.Steps;

public class RefineToneStep : IWorkflowStep
{
	public string stepType => "REFINE_TONE";

	public string execute(StepExecutionContext stepExecutionContext)
	{
		string tone = stepExecutionContext.stepDefinition.configuration
			.getValueOrDefault("tone", "professional");

		return $"{stepExecutionContext.input} [Tone refined: {tone}]";
	}
}
