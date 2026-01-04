namespace TumblrApiReader.Helpers;

public class ValidationHelper
{
    public static bool TryParseRange(string? rangeInput, out int start, out int end)
    {
        start = 0;
        end = 0;

        if (string.IsNullOrWhiteSpace(rangeInput))
            return false;

        var parts = rangeInput.Split('-');
        if (parts.Length != 2)
            return false;

        return int.TryParse(parts[0].Trim(), out start) && 
               int.TryParse(parts[1].Trim(), out end);
    }
    public static bool ValidateRange(int start, int end, int totalPosts)
    {
        return start >= 1 && start <= end && end <= totalPosts;
    }
}
