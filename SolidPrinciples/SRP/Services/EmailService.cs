using SRP.Models;

namespace SRP.Services;

public class EmailService
{
    public void SendWelcomeEmail(User user)
    {
        Console.WriteLine($"[Email] Sending welcome email to {user.Email}");
        Console.WriteLine($"[Email] Subject: Welcome {user.Name}!");
    }

    public void SendGoodbyeEmail(string email)
    {
        Console.WriteLine($"[Email] Sending goodbye email to {email}");
    }
}
