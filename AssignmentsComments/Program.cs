using TumblrApiReader.Helpers;
using TumblrApiReader.Services;

namespace TumblrApiReader;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Get blog name from user input
            Console.Write("Enter Tumblr blog name: ");
            string? blogName = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(blogName))
            {
                Console.WriteLine("Error: Blog name cannot be empty.");
                return;
            }

            // Get post range in format "start-end" (e.g., "1-5")
            Console.Write("Enter post range (start-end, e.g., 1-5): ");
            string? rangeInput = Console.ReadLine();
            
            // Validate and parse the range input
            if (!ValidationHelper.TryParseRange(rangeInput, out int start, out int end))
            {
                Console.WriteLine("Error: Invalid range format. Use format: start-end (e.g., 1-5)");
                return;
            }

            // Initialize the Tumblr API service
            var tumblrService = new TumblrApiService();

            // Fetch blog metadata to get total post count and blog info
            var metadata = await tumblrService.FetchBlogMetadata(blogName);
            if (metadata == null)
            {
                Console.WriteLine($"Error: Unable to fetch blog metadata for '{blogName}'. Blog may not exist.");
                return;
            }

            int totalPosts = metadata.Value.TotalPosts;

            // Ensure the requested range is within the blog's post count
            if (!ValidationHelper.ValidateRange(start, end, totalPosts))
            {
                Console.WriteLine($"Error: Invalid range. Must be 1 ≤ start ≤ end ≤ {totalPosts}");
                return;
            }

            // Fetch posts in the specified range
            var posts = await tumblrService.FetchAllPosts(blogName, start, end);

            // Display results
            DisplayHelper.DisplayBlogInfo(metadata.Value);
            DisplayHelper.DisplayImages(posts, start);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }
}
