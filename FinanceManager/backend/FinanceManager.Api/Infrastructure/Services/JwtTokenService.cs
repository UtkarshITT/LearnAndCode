using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FinanceManager.Application.Interfaces.Services;
using FinanceManager.Core.Entities;
using FinanceManager.Infrastructure.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FinanceManager.Infrastructure.Services;

public class JwtTokenService : ITokenService
{
	private readonly JwtSettings jwtSettings;

	public JwtTokenService(JwtSettings jwtSettings)
	{
		this.jwtSettings = jwtSettings ?? throw new ArgumentNullException(nameof(jwtSettings));
	}

	public string GenerateAccessToken(User user)
	{
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

		var claims = new[]
		{
			new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
			new Claim(ClaimTypes.Name, user.Username)
		};

		var token = new JwtSecurityToken(
			issuer: jwtSettings.Issuer,
			audience: jwtSettings.Audience,
			claims: claims,
			expires: GetAccessTokenExpiry(),
			signingCredentials: credentials);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}

	public string GenerateRefreshToken()
	{
		var bytes = new byte[64];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(bytes);
		return Convert.ToBase64String(bytes);
	}

	public string HashRefreshToken(string plainToken)
	{
		var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(plainToken));
		return Convert.ToHexString(bytes);
	}

	public bool ValidateAccessToken(string token)
	{
		try
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));
			var handler = new JwtSecurityTokenHandler();

			handler.ValidateToken(token, new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = key,
				ValidateIssuer = true,
				ValidIssuer = jwtSettings.Issuer,
				ValidateAudience = true,
				ValidAudience = jwtSettings.Audience,
				ValidateLifetime = true,
				ClockSkew = TimeSpan.Zero
			}, out SecurityToken validatedToken);

			return validatedToken is JwtSecurityToken;
		}
		catch
		{
			return false;
		}
	}

	public int? GetUserIdFromToken(string token)
	{
		try
		{
			var handler = new JwtSecurityTokenHandler();
			var jwtToken = handler.ReadJwtToken(token);
			var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

			if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
			{
				return userId;
			}

			return null;
		}
		catch
		{
			return null;
		}
	}

	public DateTime GetAccessTokenExpiry()
	{
		return DateTime.UtcNow.AddMinutes(jwtSettings.ExpirationMinutes);
	}
}
