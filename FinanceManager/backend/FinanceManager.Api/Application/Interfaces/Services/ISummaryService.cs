using FinanceManager.Application.DTOs;

namespace FinanceManager.Application.Interfaces.Services;

public interface ISummaryService
{
	Task<SummaryDto> GetSummaryAsync(int userId);

	Task<IReadOnlyList<CategorySummaryDto>> GetByCategoryAsync(int userId);

	Task<decimal> GetBalanceAsync(int userId, DateTime? from = null, DateTime? to = null);
}
