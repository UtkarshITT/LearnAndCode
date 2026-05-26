using FinanceManager.Application.DTOs;
using FinanceManager.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
	private readonly IAuthService authService;

	public AuthController(IAuthService authService)
	{
		this.authService = authService;
	}

	[HttpPost("register")]
	public async Task<ActionResult<AuthTokensDto>> Register([FromBody] LoginRequest request)
	{
		try
		{
			var tokens = await authService.RegisterAsync(request.Username, request.Password);
			return Ok(tokens);
		}
		catch (InvalidOperationException ex)
		{
			return Conflict(new { message = ex.Message });
		}
		catch (ArgumentException ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}

	[HttpPost("login")]
	public async Task<ActionResult<AuthTokensDto>> Login([FromBody] LoginRequest request)
	{
		try
		{
			var tokens = await authService.LoginAsync(request.Username, request.Password);
			return Ok(tokens);
		}
		catch (UnauthorizedAccessException ex)
		{
			return Unauthorized(new { message = ex.Message });
		}
		catch (ArgumentException ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}

	[HttpPost("refresh")]
	public async Task<ActionResult<AuthTokensDto>> Refresh([FromBody] RefreshRequest request)
	{
		try
		{
			var tokens = await authService.RefreshAsync(request.RefreshToken);
			return Ok(tokens);
		}
		catch (UnauthorizedAccessException ex)
		{
			return Unauthorized(new { message = ex.Message });
		}
	}

	[HttpPost("logout")]
	public async Task<IActionResult> Logout([FromBody] RefreshRequest request)
	{
		await authService.LogoutAsync(request.RefreshToken);
		return NoContent();
	}
}
