using FinanceManager.Application.DTOs;
using FinanceManager.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/budgets")]
public class BudgetsController : AuthorizedApiControllerBase
{
	private readonly IBudgetService budgetService;

	public BudgetsController(IBudgetService budgetService)
	{
		this.budgetService = budgetService;
	}

	[HttpGet]
	public async Task<ActionResult<IReadOnlyList<BudgetDto>>> GetAll()
	{
		return Ok(await budgetService.GetAllAsync(GetUserId()));
	}

	[HttpGet("{id:int}")]
	public async Task<ActionResult<BudgetDto>> GetById(int id)
	{
		var item = await budgetService.GetByIdAsync(GetUserId(), id);
		return item == null ? NotFound() : Ok(item);
	}

	[HttpPost]
	public async Task<ActionResult<BudgetDto>> Create([FromBody] CreateBudgetRequest request)
	{
		try
		{
			var created = await budgetService.CreateAsync(GetUserId(), request);
			return CreatedAtAction(nameof(GetById), new { id = created.BudgetId }, created);
		}
		catch (ArgumentException ex)
		{
			return BadRequest(new { message = ex.Message });
		}
		catch (InvalidOperationException ex)
		{
			return Conflict(new { message = ex.Message });
		}
	}

	[HttpPut("{id:int}")]
	public async Task<ActionResult<BudgetDto>> Update(int id, [FromBody] UpdateBudgetRequest request)
	{
		try
		{
			var updated = await budgetService.UpdateAsync(GetUserId(), id, request);
			return updated == null ? NotFound() : Ok(updated);
		}
		catch (ArgumentException ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}

	[HttpDelete("{id:int}")]
	public async Task<IActionResult> Delete(int id)
	{
		var deleted = await budgetService.DeleteAsync(GetUserId(), id);
		return deleted ? NoContent() : NotFound();
	}
}
