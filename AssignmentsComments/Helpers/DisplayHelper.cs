using TumblrApiReader.Models;

namespace TumblrApiReader.Helpers;
public class DisplayHelper
{
    public static void DisplayBlogInfo(BlogMetadata metadata)
    {
        // Display blog metadata in the required format
        Console.WriteLine("\nBlog Information:");
        Console.WriteLine($"title: {metadata.Title}");
        Console.WriteLine($"name: {metadata.Name}");
        Console.WriteLine($"description: {metadata.Description}");
        Console.WriteLine($"no of post: {metadata.TotalPosts}");
    }
    public static void DisplayImages(List<Post> posts, int startIndex)
    {
        Console.WriteLine("\nImages:");
        
        // Display images for each post, maintaining 1-based numbering
        for (int i = 0; i < posts.Count; i++)
        {
            var post = posts[i];
            
            if (post.ImageUrls.Count > 0)
            {
                // Display post number (1-based index from user's input)
                Console.WriteLine($"{startIndex + i}.");
                foreach (string imageUrl in post.ImageUrls)
                {
                    Console.WriteLine(imageUrl);
                }
                Console.WriteLine();
            }
        }
    }
}
