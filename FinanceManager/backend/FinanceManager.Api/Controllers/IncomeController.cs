using FinanceManager.Application.DTOs;
using FinanceManager.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/income")]
public class IncomeController : AuthorizedApiControllerBase
{
	private readonly IIncomeService incomeService;

	public IncomeController(IIncomeService incomeService)
	{
		this.incomeService = incomeService;
	}

	[HttpGet]
	public async Task<ActionResult<IReadOnlyList<TransactionDto>>> GetAll()
	{
		return Ok(await incomeService.GetAllAsync(GetUserId()));
	}

	[HttpGet("{id:int}")]
	public async Task<ActionResult<TransactionDto>> GetById(int id)
	{
		var item = await incomeService.GetByIdAsync(GetUserId(), id);
		return item == null ? NotFound() : Ok(item);
	}

	[HttpPost]
	public async Task<ActionResult<TransactionDto>> Create([FromBody] CreateTransactionRequest request)
	{
		try
		{
			var created = await incomeService.CreateAsync(GetUserId(), request);
			return CreatedAtAction(nameof(GetById), new { id = created.TransactionId }, created);
		}
		catch (ArgumentException ex)
		{
			return BadRequest(new { message = ex.Message });
		}
	}

	[HttpPut("{id:int}")]
	public async Task<ActionResult<TransactionDto>> Update(int id, [FromBody] UpdateTransactionRequest request)
	{
		try
		{
			var updated = await incomeService.UpdateAsync(GetUserId(), id, request);
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
		var deleted = await incomeService.DeleteAsync(GetUserId(), id);
		return deleted ? NoContent() : NotFound();
	}
}
