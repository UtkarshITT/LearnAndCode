using TumblrApiReader.Models;

namespace TumblrApiReader.Helpers;
public class DisplayHelper
{
    public static void DisplayBlogInfo(BlogMetadata metadata)
    {
        Console.WriteLine("\nBlog Information:");
        Console.WriteLine($"title: {metadata.Title}");
        Console.WriteLine($"name: {metadata.Name}");
        Console.WriteLine($"description: {metadata.Description}");
        Console.WriteLine($"no of post: {metadata.TotalPosts}");
    }
    public static void DisplayImages(List<Post> posts, int startIndex)
    {
        Console.WriteLine("\nImages:");
        
        for (int i = 0; i < posts.Count; i++)
        {
            var post = posts[i];
            
            if (post.ImageUrls.Count > 0)
            {
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
