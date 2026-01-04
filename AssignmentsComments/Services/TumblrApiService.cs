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
            string url = $"https://{blogName}.tumblr.com/api/read/json";
            string? jsonString = await _jsonHelper.ReadTumblrJson(url, _httpClient);
            
            if (string.IsNullOrEmpty(jsonString))
                return null;

            using JsonDocument doc = JsonDocument.Parse(jsonString);
            JsonElement root = doc.RootElement;

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
            return null;
        }
    }

    public async Task<List<Post>> FetchAllPosts(string blogName, int start, int end)
    {
        var allPosts = new List<Post>();
        int postsNeeded = end - start + 1;
        int offset = start - 1; // Convert to 0-based index
        int batchSize = 50;

        try
        {
            while (allPosts.Count < postsNeeded)
            {
                int num = Math.Min(batchSize, postsNeeded - allPosts.Count);
                string url = $"https://{blogName}.tumblr.com/api/read/json?start={offset}&num={num}";
                
                string? jsonString = await _jsonHelper.ReadTumblrJson(url, _httpClient);
                if (string.IsNullOrEmpty(jsonString))
                    break;

                using JsonDocument doc = JsonDocument.Parse(jsonString);
                JsonElement root = doc.RootElement;

                if (!root.TryGetProperty("posts", out JsonElement postsArray))
                    break;

                foreach (JsonElement postElement in postsArray.EnumerateArray())
                {
                    var post = new Post();
                    
                    if (postElement.TryGetProperty("photos", out JsonElement photos))
                    {
                        foreach (JsonElement photo in photos.EnumerateArray())
                        {
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

                offset += num;

                if (postsArray.GetArrayLength() < num)
                    break;
            }
        }
        catch (Exception)
        {
            
        }

        return allPosts;
    }
}
