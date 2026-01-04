using System.Text.RegularExpressions;

namespace TumblrApiReader.Helpers;
public class JsonHelper
{
    public async Task<string?> ReadTumblrJson(string url, HttpClient httpClient)
    {
        try
        {
            string response = await httpClient.GetStringAsync(url);
            
            Console.WriteLine($"\nDEBUG - First 200 chars of response:");
            Console.WriteLine(response.Length > 200 ? response.Substring(0, 200) : response);
            Console.WriteLine($"Response length: {response.Length} characters\n");
            
            var match = Regex.Match(response, @"var tumblr_api_read = (.+);?\s*$", RegexOptions.Singleline);
            
            if (match.Success && match.Groups.Count > 1)
            {
                return match.Groups[1].Value.Trim();
            }
            
            Console.WriteLine("ERROR: Could not find 'var tumblr_api_read' pattern in response");
            return null;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"ERROR: HTTP request failed: {ex.Message}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: Unexpected error: {ex.Message}");
            return null;
        }
    }
}
