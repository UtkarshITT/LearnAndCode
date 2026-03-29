using SRP.Models;

namespace SRP.Repositories;
public class UserRepository
{
    public void Save(User user)
    {
        Console.WriteLine($"[Database] Saving user: {user.Name} ({user.Email})");
    }

    public void Delete(string email)
    {
        Console.WriteLine($"[Database] Deleting user with email: {email}");
    }

    public User? GetByEmail(string email)
    {
        Console.WriteLine($"[Database] Fetching user with email: {email}");
        return new User("Sample User", email);
    }
}
