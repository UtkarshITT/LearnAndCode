namespace DIP.Interfaces;
public interface IDataRepository
{
    void Save(string data);
    string? Get(string id);
    IEnumerable<string> GetAll();
}
