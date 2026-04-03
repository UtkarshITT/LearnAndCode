using System;

namespace ExceptionalHandling.Exceptions;

public class InvalidOrderAmountException : Exception
{
    public InvalidOrderAmountException(string message) : base(message)
    {
    }
}
