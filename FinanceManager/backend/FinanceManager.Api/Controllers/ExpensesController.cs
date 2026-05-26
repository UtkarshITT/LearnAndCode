using FinanceManager.Application.DTOs;
using FinanceManager.Application.Enums;
using FinanceManager.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceManager.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/expenses")]
public class ExpensesController : AuthorizedApiControllerBase
{
	private readonly IExpenseService expenseService;

	public ExpensesController(IExpenseService expenseService)
	{
		this.expenseService = expenseService;
	}

	[HttpGet]
	public async Task<ActionResult<IReadOnlyList<TransactionDto>>> GetAll(
		[FromQuery] string? keyword,
		[FromQuery] ExpenseSortField sortBy = ExpenseSortField.Date,
		[FromQuery] SortDirection sortDirection = SortDirection.Desc)
	{
		var items = await expenseService.GetAllAsync(GetUserId(), keyword, sortBy, sortDirection);
		return Ok(items);
	}

	[HttpGet("{id:int}")]
	public async Task<ActionResult<TransactionDto>> GetById(int id)
	{
		var item = await expenseService.GetByIdAsync(GetUserId(), id);
		return item == null ? NotFound() : Ok(item);
	}

	[HttpPost]
	public async Task<ActionResult<TransactionDto>> Create([FromBody] CreateTransactionRequest request)
	{
		try
		{
			var created = await expenseService.CreateAsync(GetUserId(), request);
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
			var updated = await expenseService.UpdateAsync(GetUserId(), id, request);
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
		var deleted = await expenseService.DeleteAsync(GetUserId(), id);
		return deleted ? NoContent() : NotFound();
	}
}
