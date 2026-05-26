using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.Api.Controllers;

public abstract class AuthorizedApiControllerBase : ControllerBase
{
	protected int GetUserId()
	{
		var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
		return int.Parse(claim ?? throw new UnauthorizedAccessException());
	}
}
