using DIP.Interfaces;

namespace DIP.Implementations;
public class SqlRepository : IDataRepository
{
    private readonly List<string> _data = new();

    public void Save(string data)
    {
        _data.Add(data);
        Console.WriteLine($"[SQL Server] Saved: {data}");
    }

    public string? Get(string id)
    {
        Console.WriteLine($"[SQL Server] Fetching: {id}");
        return _data.FirstOrDefault(d => d.Contains(id));
    }

    public IEnumerable<string> GetAll()
    {
        Console.WriteLine("[SQL Server] Fetching all records");
        return _data;
    }
}
