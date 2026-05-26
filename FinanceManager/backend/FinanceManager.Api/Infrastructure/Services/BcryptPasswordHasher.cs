using BCrypt.Net;
using FinanceManager.Application.Interfaces.Services;

namespace FinanceManager.Infrastructure.Services;

public class BcryptPasswordHasher : IPasswordHasher
{
	public string HashPassword(string password)
	{
		return BCrypt.Net.BCrypt.HashPassword(password);
	}

	public bool VerifyPassword(string password, string hash)
	{
		return BCrypt.Net.BCrypt.Verify(password, hash);
	}
}
