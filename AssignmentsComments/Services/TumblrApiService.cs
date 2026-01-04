using System.Text.Json;
using TumblrApiReader.Helpers;
using TumblrApiReader.Models;

namespace TumblrApiReader.Services;
public class TumblrApiService
{
    private readonly HttpClient _httpClient;
    private readonly JsonHelper _jsonHelper;

    public TumblrApiService()
    {
        _httpClient = new HttpClient();
        _jsonHelper = new JsonHelper();
    }
    public async Task<BlogMetadata?> FetchBlogMetadata(string blogName)
    {
        try
        {
            // Construct API URL using Tumblr v1 API endpoint
            string url = $"https://{blogName}.tumblr.com/api/read/json";
            string? jsonString = await _jsonHelper.ReadTumblrJson(url, _httpClient);
            
            if (string.IsNullOrEmpty(jsonString))
                return null;

            // Parse JSON response and extract blog metadata
            using JsonDocument doc = JsonDocument.Parse(jsonString);
            JsonElement root = doc.RootElement;

            // Map JSON properties to BlogMetadata object
            return new BlogMetadata
            {
                Title = root.GetProperty("tumblelog").GetProperty("title").GetString() ?? "",
                Name = root.GetProperty("tumblelog").GetProperty("name").GetString() ?? "",
                Description = root.GetProperty("tumblelog").GetProperty("description").GetString() ?? "",
                TotalPosts = root.GetProperty("posts-total").GetInt32()
            };
        }
        catch (Exception)
        {
            // Return null if any error occurs during fetching or parsing
            return null;
        }
    }

    public async Task<List<Post>> FetchAllPosts(string blogName, int start, int end)
    {
        var allPosts = new List<Post>();
        int postsNeeded = end - start + 1;
        int offset = start - 1; // Convert 1-based user input to 0-based API offset
        int batchSize = 50; // Tumblr API limit per request

        try
        {
            // Pagination loop: fetch posts in batches until we have all requested posts
            while (allPosts.Count < postsNeeded)
            {
                // Calculate how many posts to request in this batch
                int num = Math.Min(batchSize, postsNeeded - allPosts.Count);
                string url = $"https://{blogName}.tumblr.com/api/read/json?start={offset}&num={num}";
                
                string? jsonString = await _jsonHelper.ReadTumblrJson(url, _httpClient);
                if (string.IsNullOrEmpty(jsonString))
                    break;

                using JsonDocument doc = JsonDocument.Parse(jsonString);
                JsonElement root = doc.RootElement;

                if (!root.TryGetProperty("posts", out JsonElement postsArray))
                    break;

                // Process each post in the batch
                foreach (JsonElement postElement in postsArray.EnumerateArray())
                {
                    var post = new Post();
                    
                    // Extract photos if the post contains any
                    if (postElement.TryGetProperty("photos", out JsonElement photos))
                    {
                        foreach (JsonElement photo in photos.EnumerateArray())
                        {
                            // Get highest quality image (1280px version)
                            if (photo.TryGetProperty("photo-url-1280", out JsonElement photoUrl))
                            {
                                string? imageUrl = photoUrl.GetString();
                                if (!string.IsNullOrEmpty(imageUrl))
                                {
                                    post.ImageUrls.Add(imageUrl);
                                }
                            }
                        }
                    }
                    
                    allPosts.Add(post);
                }

                // Move offset forward for next batch
                offset += num;

                // Stop if we received fewer posts than requested (reached end of blog)
                if (postsArray.GetArrayLength() < num)
                    break;
            }
        }
        catch (Exception)
        {
            // Return whatever posts were successfully fetched before the error
        }

        return allPosts;
    }
}
