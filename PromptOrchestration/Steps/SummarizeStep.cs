namespace PromptOrchestration.Steps;

public class SummarizeStep : IWorkflowStep
{
	private const int summaryLength = 60;

	public string stepType => "SUMMARIZE";

	public string execute(StepExecutionContext stepExecutionContext)
	{
		string input = stepExecutionContext.input;
		string summary = input.Length <= summaryLength
			? input
			: input[..summaryLength] + "...";

		return $"Summary: {summary}";
	}
}
