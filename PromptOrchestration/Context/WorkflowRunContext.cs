namespace PromptOrchestration;

public class WorkflowRunContext
{
	private readonly Dictionary<string, string> metadata;

	public string runId { get; }

	public string workflowName { get; }

	public DateTime startedAtUtc { get; }

	public WorkflowRunContext(string workflowName, IReadOnlyDictionary<string, string>? metadata = null)
	{
		this.workflowName = workflowName;
		runId = Guid.NewGuid().ToString("N");
		startedAtUtc = DateTime.UtcNow;
		this.metadata = metadata is null
			? new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
			: new Dictionary<string, string>(metadata, StringComparer.OrdinalIgnoreCase);
	}

	public string getMetadataOrDefault(string key, string defaultValue = "")
	{
		if (metadata.TryGetValue(key, out string? value))
		{
			return value;
		}

		return defaultValue;
	}
}
