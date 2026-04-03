namespace ExceptionalhandlingAssessments.Exceptions;

public class InvalidWithdrawalAmountException : Exception
{
	public InvalidWithdrawalAmountException(string message) : base(message)
	{
	}
}
