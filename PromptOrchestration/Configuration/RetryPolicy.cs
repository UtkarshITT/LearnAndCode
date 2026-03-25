namespace PromptOrchestration;

public class RetryPolicy
{
	private const int minimumAttempts = 1;
	private const int minimumDelayMilliseconds = 0;

	public int maxAttempts { get; }

	public int delayMilliseconds { get; }

	public RetryPolicy(int maxAttempts, int delayMilliseconds)
	{
		if (maxAttempts < minimumAttempts)
		{
			throw new ArgumentOutOfRangeException(nameof(maxAttempts), "maxAttempts must be at least 1.");
		}

		if (delayMilliseconds < minimumDelayMilliseconds)
		{
			throw new ArgumentOutOfRangeException(
				nameof(delayMilliseconds),
				"delayMilliseconds must not be negative.");
		}

		this.maxAttempts = maxAttempts;
		this.delayMilliseconds = delayMilliseconds;
	}

	public static RetryPolicy noRetry()
	{
		return new RetryPolicy(1, 0);
	}
}
