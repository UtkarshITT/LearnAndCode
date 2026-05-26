using FinanceManager.Application.DTOs;
using FinanceManager.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/summary")]
public class SummaryController : AuthorizedApiControllerBase
{
	private readonly ISummaryService summaryService;

	public SummaryController(ISummaryService summaryService)
	{
		this.summaryService = summaryService;
	}

	[HttpGet]
	public async Task<ActionResult<SummaryDto>> GetSummary()
	{
		return Ok(await summaryService.GetSummaryAsync(GetUserId()));
	}

	[HttpGet("by-category")]
	public async Task<ActionResult<IReadOnlyList<CategorySummaryDto>>> GetByCategory()
	{
		return Ok(await summaryService.GetByCategoryAsync(GetUserId()));
	}

	[HttpGet("balance")]
	public async Task<ActionResult<decimal>> GetBalance([FromQuery] DateTime? from, [FromQuery] DateTime? to)
	{
		return Ok(await summaryService.GetBalanceAsync(GetUserId(), from, to));
	}
}
