namespace PromptOrchestration.Steps;

public class TranslateStep : IWorkflowStep
{
	public string stepType => "TRANSLATE";

	public string execute(StepExecutionContext stepExecutionContext)
	{
		string targetLanguage = stepExecutionContext.stepDefinition.configuration
			.getValueOrDefault("targetLanguage", "English");

		return $"Translated ({targetLanguage}): {stepExecutionContext.input}";
	}
}
