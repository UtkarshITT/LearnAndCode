using TumblrApiReader.Helpers;
using TumblrApiReader.Services;

namespace TumblrApiReader;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            Console.Write("Enter Tumblr blog name: ");
            string? blogName = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(blogName))
            {
                Console.WriteLine("Error: Blog name cannot be empty.");
                return;
            }

            Console.Write("Enter post range (start-end, e.g., 1-5): ");
            string? rangeInput = Console.ReadLine();
            
            if (!ValidationHelper.TryParseRange(rangeInput, out int start, out int end))
            {
                Console.WriteLine("Error: Invalid range format. Use format: start-end (e.g., 1-5)");
                return;
            }

            var tumblrService = new TumblrApiService();

            var metadata = await tumblrService.FetchBlogMetadata(blogName);
            if (metadata == null)
            {
                Console.WriteLine($"Error: Unable to fetch blog metadata for '{blogName}'. Blog may not exist.");
                return;
            }

            int totalPosts = metadata.Value.TotalPosts;

            if (!ValidationHelper.ValidateRange(start, end, totalPosts))
            {
                Console.WriteLine($"Error: Invalid range. Must be 1 ≤ start ≤ end ≤ {totalPosts}");
                return;
            }

            var posts = await tumblrService.FetchAllPosts(blogName, start, end);

            DisplayHelper.DisplayBlogInfo(metadata.Value);
            DisplayHelper.DisplayImages(posts, start);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }
}
