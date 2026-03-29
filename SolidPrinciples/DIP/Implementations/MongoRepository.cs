using DIP.Interfaces;

namespace DIP.Implementations;
public class MongoRepository : IDataRepository
{
    private readonly List<string> _data = new();

    public void Save(string data)
    {
        _data.Add(data);
        Console.WriteLine($"[MongoDB] Saved: {data}");
    }

    public string? Get(string id)
    {
        Console.WriteLine($"[MongoDB] Fetching: {id}");
        return _data.FirstOrDefault(d => d.Contains(id));
    }

    public IEnumerable<string> GetAll()
    {
        Console.WriteLine("[MongoDB] Fetching all documents");
        return _data;
    }
}
