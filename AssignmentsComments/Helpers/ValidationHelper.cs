namespace TumblrApiReader.Helpers;

public class ValidationHelper
{
    public static bool TryParseRange(string? rangeInput, out int start, out int end)
    {
        start = 0;
        end = 0;

        if (string.IsNullOrWhiteSpace(rangeInput))
            return false;

        // Expected format: "start-end" (e.g., "1-5")
        var parts = rangeInput.Split('-');
        if (parts.Length != 2)
            return false;

        // Parse both parts as integers
        return int.TryParse(parts[0].Trim(), out start) && 
               int.TryParse(parts[1].Trim(), out end);
    }
    public static bool ValidateRange(int start, int end, int totalPosts)
    {
        // Ensure: start >= 1 (1-based indexing), start <= end, and end within blog's post count
        return start >= 1 && start <= end && end <= totalPosts;
    }
}
