using SRP.Models;
using SRP.Repositories;

namespace SRP.Services;
public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly EmailService _emailService;
    private readonly Logger _logger;

    public UserService(UserRepository userRepository, EmailService emailService, Logger logger)
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _logger = logger;
    }

    public void CreateUser(string name, string email)
    {
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email))
        {
            throw new ArgumentException("Name and email are required");
        }

        var user = new User(name, email);

        _userRepository.Save(user);
        _emailService.SendWelcomeEmail(user);
        _logger.Log($"User created: {name}");
    }

    public void DeleteUser(string email)
    {
        _userRepository.Delete(email);
        _emailService.SendGoodbyeEmail(email);
        _logger.Log($"User deleted: {email}");
    }
}
